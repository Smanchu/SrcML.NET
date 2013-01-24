﻿/******************************************************************************
 * Copyright (c) 2011 ABB Group
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *    Vinay Augustine (ABB Group) - Initial implementation
 *    Jiang Zheng (ABB Group) - Initial implementation
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using ABB.SrcML.Utilities;

namespace ABB.SrcML
{
    ////public class SrcMLArchive : AbstractArchive, ISourceFolder
    public class SrcMLArchive : AbstractArchive, ISrcMLDOTNETEvents
    {
        ////public SrcMLArchive(ISourceFolder sourceDirectory)
        public SrcMLArchive(ISolutionMonitorEvents solutionMonitor)
            : this(solutionMonitor, Path.Combine(solutionMonitor.FullFolderPath, ".srcml"), new Src2SrcMLRunner())
        {

        }

        ////public SrcMLArchive(ISourceFolder sourceDirectory, string xmlDirectory)
        public SrcMLArchive(ISolutionMonitorEvents solutionMonitor, string xmlDirectory)
            : this(solutionMonitor, xmlDirectory, new Src2SrcMLRunner())
        {

        }

        ////public SrcMLArchive(ISourceFolder sourceDirectory, string xmlDirectory, Src2SrcMLRunner generator)
        public SrcMLArchive(ISolutionMonitorEvents solutionMonitor, string xmlDirectory, Src2SrcMLRunner generator)
        {
            this.SolutionMonitor = solutionMonitor;
            this.ArchivePath = xmlDirectory;
            this.XmlGenerator = generator;
            
            if (!Directory.Exists(this.ArchivePath))
            {
                Directory.CreateDirectory(this.ArchivePath);
            }

            InitializeValidFileExtensions();

            ////this.SourceDirectory.SourceFileChanged += RespondToFileChangedEvent;
            this.SolutionMonitor.SolutionMonitorEventRaised += RespondToSolutionMonitorEvent;
        }

        ////public ISourceFolder SourceDirectory
        public ISolutionMonitorEvents SolutionMonitor
        {
            get;
            set;
        }

        public Src2SrcMLRunner XmlGenerator
        {
            get;
            set;
        }

        /// <summary>
        /// Background workers.
        /// </summary>
        //private readonly BackgroundWorker _processFileInBackground;   // no use so far, will consider this later
        private BackgroundWorker startupWorker;

        /// <summary>
        /// Set of source file types that can be processed by SrcML.NET.
        /// </summary>
        private HashSet<string> validFileExtensions;

        #region ISrcMLDOTNETEvents Members

        public event EventHandler<SrcMLDOTNETEventArgs> SrcMLDOTNETEventRaised;

        public string FullFolderPath
        {
            get
            {
                return this.SolutionMonitor.FullFolderPath;
            }
            set
            {
                this.SolutionMonitor.FullFolderPath = value;
            }
        }

        public void StartWatching()
        {
            // run background thread for startup
            writeLog("D:\\Data\\log.txt", "======= SrcMLArchive: START WATCHING =======");
            startupWorker = new BackgroundWorker();
            startupWorker.WorkerSupportsCancellation = true;
            startupWorker.DoWork += new DoWorkEventHandler(_runStartupInBackground_DoWork);
            startupWorker.RunWorkerAsync();
            
            this.SolutionMonitor.StartWatching();
        }

        public void StopWatching()
        {
            writeLog("D:\\Data\\log.txt", "======= SrcMLArchive: STOP WATCHING =======");
            try
            {
                this.SolutionMonitor.StopWatching();

                // Disable the startup background worker
                if (startupWorker != null)
                {
                    startupWorker.CancelAsync();
                }

                // Disable the file processing background worker
                //if (_processFileInBackground != null)
                //{
                    //_processFileInBackground.CancelAsync();
                //}
            }
            finally
            {
                // maybe not necessary
                writeLog("D:\\Data\\log.txt", "Raise a MonitoringStopped event.");
                OnSrcMLDOTNETEventsRaised(new SrcMLDOTNETEventArgs(null, SrcMLDOTNETEventType.MonitoringStopped));
            }
        }

        #endregion

        /* //// Original code
        #region ISourceFolder Members

        public event EventHandler<SourceEventArgs> SourceFileChanged;

        public string FullFolderPath
        {
            get
            {
                return this.SourceDirectory.FullFolderPath;
            }
            set
            {
                this.SourceDirectory.FullFolderPath = value;
            }
        }

        public void StartWatching()
        {
            this.SourceDirectory.StartWatching();
        }

        public void StopWatching()
        {
            this.SourceDirectory.StopWatching();
        }

        #endregion
        */

        #region AbstractArchive Members

        public override IEnumerable<XElement> FileUnits
        {
            get
            {
                var xmlFiles = Directory.EnumerateFiles(this.ArchivePath, "*.xml", SearchOption.AllDirectories);
                foreach (var xmlFileName in xmlFiles)
                {
                    yield return XElement.Load(xmlFileName, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                }
            }
        }

        public override void AddUnits(IEnumerable<XElement> units)
        {
            foreach (var unit in units)
            {
                var path = this.GetPathForUnit(unit);
                var xmlPath = this.GetXmlPathForSourcePath(path);
                unit.Save(xmlPath, SaveOptions.DisableFormatting);
            }
        }

        public override void DeleteUnits(IEnumerable<XElement> units)
        {
            foreach (var unit in units)
            {
                var path = this.GetPathForUnit(unit);
                DeleteXmlForSourceFile(path);
            }
        }

        public override void UpdateUnits(IEnumerable<XElement> units)
        {
            foreach (var unit in units)
            {
                var path = this.GetPathForUnit(unit);
                var xmlPath = this.GetXmlPathForSourcePath(path);
                unit.Save(xmlPath, SaveOptions.DisableFormatting);
            }
        }

        public override XElement GetUnitForPath(string pathToUnit)
        {
            throw new NotImplementedException();
        }
        #endregion


        /// <summary>
        /// Initialize the set of source file types that can be processed by SrcML.NET.
        /// </summary>
        private void InitializeValidFileExtensions()
        {
            validFileExtensions = new HashSet<string> { ".c", ".cpp", ".cs", ".cxx", ".h", ".java" };
        }

        /// <summary>
        /// Run in background when starting up the solution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="anEvent"></param>
        private void _runStartupInBackground_DoWork(object sender, DoWorkEventArgs anEvent)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var worker = sender as BackgroundWorker;
            try
            {
                // compare two lists ??
                List<string> allMonitoredFiles = SolutionMonitor.GetAllMonitoredFiles(worker);
                writeLog("D:\\Data\\log.txt", "@@@ allMonitoredFiles [" + allMonitoredFiles.Count + "]");
                foreach (string sourceFilePath in allMonitoredFiles)
                {
                    ProcessSingleSourceFile(sourceFilePath);
                }

                List<string> allSrcMLedFiles = GetAllSrcMLedFiles();
                writeLog("D:\\Data\\log.txt", "@@@ allSrcMLedFiles [" + allSrcMLedFiles.Count + "]");
                foreach (string sourceFilePath in allSrcMLedFiles)
                {
                    ProcessSingleSourceFile(sourceFilePath);
                }
            }
            catch (Exception e)
            {
                writeLog("D:\\Data\\log.txt", "Startup exception: " + e.Message);
            }
            finally
            {
                OnSrcMLDOTNETEventsRaised(new SrcMLDOTNETEventArgs(null, SrcMLDOTNETEventType.StartupCompleted));
            }

            stopwatch.Stop();
            writeLog("D:\\Data\\log.txt", "Total time for startup check: " + stopwatch.Elapsed.ToString());
        }

        /// <summary>
        /// Get all "SrcMLed" files in this solution.
        /// TODO: maybe use KeyValuePairs instead of List for better performance
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllSrcMLedFiles()
        {
            List<string> allSrcMLedFiles = new List<string>();
            DirectoryInfo srcMLDir = new DirectoryInfo(Path.GetFullPath(this.ArchivePath));
            FileInfo[] srcMLFiles = null;
            try
            {
                srcMLFiles = srcMLDir.GetFiles("*.*");
            }
            // In case one of the files requires permissions greater than the application provides
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (srcMLFiles != null)
            {
                foreach (FileInfo fi in srcMLFiles)
                {
                    string sourceFilePath = GetSourcePathForXmlPath(fi.Name);
                    writeLog("D:\\Data\\log.txt", "GetAllSrcMLedFileNames() " + sourceFilePath);
                    allSrcMLedFiles.Add(sourceFilePath);
                }
            }
            return allSrcMLedFiles;
        }

        /// <summary>
        /// Process a single source file to add or change the corresponding srcML file, or do nothing.
        /// TODO: GetXmlPathForSourcePath() twice
        /// </summary>
        /// <param name="sourceFilePath"></param>
        public void ProcessSingleSourceFile(string sourceFilePath)
        {
            if (isValidFileExtension(sourceFilePath))
            {
                if (!File.Exists(sourceFilePath))
                {
                    // If there is not such a source file, then delete the corresponding srcML file
                    writeLog("D:\\Data\\log.txt", "--> To DELETE srcML for: " + sourceFilePath);
                    RespondToSolutionMonitorEvent(null, new SolutionMonitorEventArgs(sourceFilePath, SolutionMonitorEventType.FileDeleted));
                }
                else
                {
                    string srcMLFilePath = GetXmlPathForSourcePath(sourceFilePath);
                    writeLog("D:\\Data\\log.txt", "ProcessSingleSourceFile(): src = [" + sourceFilePath + "], srcML = [" + srcMLFilePath + "]");
                    if (!File.Exists(srcMLFilePath))
                    {
                        // If there is not a corresponding srcML file, then generate the srcML file
                        writeLog("D:\\Data\\log.txt", "--> To ADD: " + srcMLFilePath);
                        RespondToSolutionMonitorEvent(null, new SolutionMonitorEventArgs(sourceFilePath, SolutionMonitorEventType.FileAdded));
                    }
                    else
                    {
                        DateTime sourceFileTimestamp = new FileInfo(sourceFilePath).LastWriteTime;
                        DateTime srcLMFileTimestamp = new FileInfo(srcMLFilePath).LastWriteTime;
                        if (sourceFileTimestamp.CompareTo(srcLMFileTimestamp) > 0)
                        {
                            // If source file's timestamp is later than its srcML file's timestamp, then generate the srcML file, otherwise do nothing
                            writeLog("D:\\Data\\log.txt", "--> To CHANGE: " + srcMLFilePath);
                            RespondToSolutionMonitorEvent(null, new SolutionMonitorEventArgs(sourceFilePath, SolutionMonitorEventType.FileChanged));
                        }
                        else
                        {
                            writeLog("D:\\Data\\log.txt", "--> NO ACTION: " + sourceFilePath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if the file extension is in the set of file types that can be processed by SrcML.NET.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool isValidFileExtension(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);
            writeLog("D:\\Data\\log.txt", "fileExtension: [" + fileExtension + "]");
            if (fileExtension != null && !fileExtension.Equals(String.Empty))
            {
                if (validFileExtensions.Contains(fileExtension))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Respond to an event raised from Solution Monitor. Then raise a new event to client application (e.g., Sando)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        ////public void RespondToFileChangedEvent(object sender, SourceEventArgs eventArgs)
        public void RespondToSolutionMonitorEvent(object sender, SolutionMonitorEventArgs eventArgs)
        {
            string sourceFilePath = eventArgs.SourceFilePath;
            string oldSourceFilePath = eventArgs.OldSourceFilePath;

            var directoryName = Path.GetDirectoryName(Path.GetFullPath(sourceFilePath));
            var xmlFullPath = Path.GetFullPath(this.ArchivePath);
            
            if (!directoryName.StartsWith(xmlFullPath, StringComparison.InvariantCultureIgnoreCase))
            {
                XElement xelement = null;
                SrcMLDOTNETEventArgs srcMLDOTNETEventArgs = null;
                switch (eventArgs.EventType)
                {
                    case SolutionMonitorEventType.FileAdded:
                        xelement = GenerateXmlAndXElementForSource(sourceFilePath);
                        srcMLDOTNETEventArgs = new SrcMLDOTNETEventArgs(sourceFilePath, xelement, SrcMLDOTNETEventType.SourceFileAdded);
                        break;
                    case SolutionMonitorEventType.FileChanged:
                        xelement = GenerateXmlAndXElementForSource(sourceFilePath);
                        srcMLDOTNETEventArgs = new SrcMLDOTNETEventArgs(sourceFilePath, xelement, SrcMLDOTNETEventType.SourceFileChanged);
                        break;
                    case SolutionMonitorEventType.FileDeleted:
                        DeleteXmlForSourceFile(sourceFilePath);
                        srcMLDOTNETEventArgs = new SrcMLDOTNETEventArgs(sourceFilePath, xelement, SrcMLDOTNETEventType.SourceFileDeleted);
                        break;
                    case SolutionMonitorEventType.FileRenamed:
                        DeleteXmlForSourceFile(oldSourceFilePath);
                        xelement = GenerateXmlAndXElementForSource(sourceFilePath);
                        srcMLDOTNETEventArgs = new SrcMLDOTNETEventArgs(sourceFilePath, oldSourceFilePath, xelement, SrcMLDOTNETEventType.SourceFileRenamed);
                        break;
                }

                writeLog("D:\\Data\\log.txt", "SrcMLArchive raises an event for [" + sourceFilePath + ".");
                OnSourceFileChanged(srcMLDOTNETEventArgs);
            }
        }

        /// <summary>
        /// Regenerate srcML files only for added/changed/deleted/renamed files under a directory recursively.
        /// Last modified on 2012.10.11
        /// </summary>
        /// <param name="directoryPath"></param>
        /*
        public void GenerateXmlForDirectory(string directoryPath)
        {
            // Traverse source directory to generate srcML files when needed (TODO: make sure directoryPath is a full path?)
            DirectoryInfo rootDir = new DirectoryInfo(directoryPath);
            WalkSourceDirectoryTree(rootDir);

            // Traverse srcML directory to remove srcML files when needed
            //// Try to store srcML files with Base32 encoding in ONE folder
            DirectoryInfo srcMLRootDir = new DirectoryInfo(Path.GetFullPath(this.ArchivePath));
            WalkSrcMLDirectoryTree(srcMLRootDir);
        }
        */

        /* //// This functionality should be within SolutionMonitor.cs now.
        private void WalkSourceDirectoryTree(DirectoryInfo sourceDir)
        {
            FileInfo[] sourceFiles = null;
            DirectoryInfo[] sourceSubDirs = null;

            try
            {
                sourceFiles = sourceDir.GetFiles("*.*");
            }
            // In case one of the files requires permissions greater than the application provides
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (sourceFiles != null)
            {
                foreach (FileInfo fi in sourceFiles)
                {
                    ////Console.WriteLine("-> Source File: " + fi.FullName);
                    string srcMLFilePath = GetXmlPathForSourcePath(fi.FullName);
                    ////Console.WriteLine("-> srcML File: " + srcMLFilePath);
                    try
                    {
                        if (!File.Exists(srcMLFilePath))
                        {
                            // If there is not a corresponding srcML file, then generate the srcML file [Added]
                            RespondToFileChangedEvent(null, new SourceEventArgs(fi.FullName, SourceEventType.Added));
                            ////Console.WriteLine("Added");
                        }
                        else
                        {
                            // if source file's timestamp is later than its srcML file's timestamp, 
                            // then GenerateXmlForSource() [Changed]
                            DateTime sourceFileTimestamp = fi.LastWriteTime;
                            DateTime srcLMFileTimestamp = new FileInfo(srcMLFilePath).LastWriteTime;
                            if (sourceFileTimestamp.CompareTo(srcLMFileTimestamp) > 0)
                            {
                                RespondToFileChangedEvent(null, new SourceEventArgs(fi.FullName, SourceEventType.Changed));
                                Console.WriteLine("Changed");
                            }
                            else
                            {
                                //Console.WriteLine("!!! NO ACTION !!!");
                            }
                        }
                    }
                    // In case the file has been deleted since the traversal
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                sourceSubDirs = sourceDir.GetDirectories();
                foreach (DirectoryInfo sourceDirInfo in sourceSubDirs)
                {
                    //Console.WriteLine("sourceDirInfo: " + sourceDirInfo.Name);
                    if (!".srcml".Equals(sourceDirInfo.Name))
                    {
                        WalkSourceDirectoryTree(sourceDirInfo);
                    }
                }
            }
        }
        */

        /*
        private void WalkSrcMLDirectoryTree(DirectoryInfo srcMLDir)
        {
            FileInfo[] srcMLFiles = null;
            DirectoryInfo[] srcMLSubDirs = null;

            try
            {
                srcMLFiles = srcMLDir.GetFiles("*.*");
            }
            // In case one of the files requires permissions greater than the application provides
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (srcMLFiles != null)
            {
                foreach (FileInfo fi in srcMLFiles)
                {
                    Console.WriteLine("<- srcML File: " + fi.FullName);
                    string sourceFilePath = GetSourcePathForXmlPath(fi.FullName);
                    Console.WriteLine("<- Source File: " + sourceFilePath);
                    try
                    {
                        if (!File.Exists(sourceFilePath))
                        {
                            // If there is not a corresponding source file, then delete the srcML file [Deleted]
                            RespondToFileChangedEvent(null, new SourceEventArgs(sourceFilePath, SourceEventType.Deleted));
                            Console.WriteLine("Deleted");
                        }
                        else
                        {
                            //Console.WriteLine("!!! NO ACTION !!!");
                        }
                    }
                    // In case the file has been deleted since the traversal
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                srcMLSubDirs = srcMLDir.GetDirectories();
                foreach (DirectoryInfo srcMLDirInfo in srcMLSubDirs)
                {
                    //Console.WriteLine("srcMLDirInfo: " + srcMLDirInfo.Name);
                    WalkSrcMLDirectoryTree(srcMLDirInfo);
                }
            }
        }
        */

        /* //// This functionality should be within SolutionMonitor.cs now.
        private void WalkSrcMLDirectoryTree(DirectoryInfo srcMLDir)
        {
            FileInfo[] srcMLFiles = null;

            try
            {
                srcMLFiles = srcMLDir.GetFiles("*.*");
            }
            // In case one of the files requires permissions greater than the application provides
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (srcMLFiles != null)
            {
                foreach (FileInfo fi in srcMLFiles)
                {
                    ////Console.WriteLine("<- srcML File: " + fi.FullName);
                    string sourceFilePath = GetSourcePathForXmlPath(fi.FullName);
                    ////Console.WriteLine("<- Source File: " + sourceFilePath);
                    try
                    {
                        if (!File.Exists(sourceFilePath))
                        {
                            // If there is not a corresponding source file, then delete the srcML file [Deleted]
                            RespondToFileChangedEvent(null, new SourceEventArgs(sourceFilePath, SourceEventType.Deleted));
                            Console.WriteLine("Deleted");
                        }
                        else
                        {
                            //Console.WriteLine("!!! NO ACTION !!!");
                        }
                    }
                    // In case the file has been deleted since the traversal
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        */

        /// <summary>
        /// Generate both a srcML File and a XElement of the content of this file for a source code file.
        /// </summary>
        /// <param name="sourcePath">The full path of the source code file.</param>
        /// <returns>The XElement of the content of the generated srcML file.</returns>
        public XElement GenerateXmlAndXElementForSource(string sourcePath)
        {
            var xmlPath = GetXmlPathForSourcePath(sourcePath);
            var directory = Path.GetDirectoryName(xmlPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            writeLog("D:\\Data\\log.txt", "GenerateXmlAndXElementForSource(): sourcePath = [" + sourcePath + "], xmlPath = [" + xmlPath + "]");
            return this.XmlGenerator.GenerateSrcMLAndXElementFromFile(sourcePath, xmlPath);
        }

        /// <summary>
        /// Generate both a srcML File and a string of the content of this file for a source code file.
        /// </summary>
        /// <param name="sourcePath">The full path of the source code file.</param>
        /// <returns>The string of the content of the generated srcML file.</returns>
        public string GenerateXmlAndStringForSource(string sourcePath)
        {
            var xmlPath = GetXmlPathForSourcePath(sourcePath);
            var directory = Path.GetDirectoryName(xmlPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return this.XmlGenerator.GenerateSrcMLAndStringFromFile(sourcePath, xmlPath);
        }

        /// <summary>
        /// Generate a srcML File for a source code file. Now use this method instead of GenerateXmlAndXElementForSource()
        /// </summary>
        /// <param name="sourcePath"></param>
        public void GenerateXmlForSource(string sourcePath)
        {
            var xmlPath = GetXmlPathForSourcePath(sourcePath);
            var directory = Path.GetDirectoryName(xmlPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            this.XmlGenerator.GenerateSrcMLFromFile(sourcePath, xmlPath);
        }

        /// <summary>
        /// Delete the srcML file for a specified source file.
        /// </summary>
        /// <param name="sourcePath"></param>
        public void DeleteXmlForSourceFile(string sourcePath)
        {
            var xmlPath = GetXmlPathForSourcePath(sourcePath);
            writeLog("D:\\Data\\log.txt", "DeleteXmlForSource(): sourcePath = [" + sourcePath + "], xmlPath = [" + xmlPath + "]");
            var sourceDirectory = Path.GetDirectoryName(sourcePath);

            if (File.Exists(xmlPath))
            {
                File.Delete(xmlPath);
            }

            /*
            if (!Directory.Exists(sourceDirectory))
            {
                var xmlDirectory = Path.GetDirectoryName(xmlPath);
                Directory.Delete(xmlDirectory);
            }
            */
        }

        /// <summary>
        /// Get the corresponding srcML file path for a specific source file.
        /// For single folder storage algorithm
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        public string GetXmlPathForSourcePath(string sourcePath)
        {
            string fullPath = (Path.IsPathRooted(sourcePath)) ? sourcePath : Path.GetFullPath(sourcePath);
            //if (!fullPath.StartsWith(this.SourceDirectory.FullFolderPath, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    throw new IOException(String.Format("{0} is not rooted in {1}", sourcePath, this.SourceDirectory));
            //}
            //string srcMLFileName = Base32.ToBase32String(fullPath);               // Base32 encoding
            string srcMLFileName = fullPath.Replace("\\", "-").Replace(":", "=");   // Simple encoding
            string xmlPath = Path.Combine(this.ArchivePath, srcMLFileName) + ".xml";
            //writeLog("D:\\Data\\log.txt", "GetXmlPathForSourcePath(): sourcePath = [" + sourcePath + "], xmlPath = [" + xmlPath + "]");
            return xmlPath;
        }

        /* Original implementation
        public string GetXmlPathForSourcePath(string sourcePath)
        {
            string fullPath = String.Empty;
            if (Path.IsPathRooted(sourcePath))
            {
                fullPath = sourcePath;
            }
            else
            {
                fullPath = Path.GetFullPath(sourcePath);
            }

            if (!fullPath.StartsWith(this.SourceDirectory.FullFolderPath, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new IOException(String.Format("{0} is not rooted in {1}", sourcePath, this.SourceDirectory));
            }

            //Console.WriteLine("fullPath = " + fullPath);
            string srcMLFileName = Base32.ToBase32String(fullPath);
            //Console.WriteLine("Encoded srcMLFileName = " + srcMLFileName);
            string xmlPath = Path.Combine(this.ArchivePath, srcMLFileName);
            xmlPath = xmlPath + ".xml";

            return xmlPath;
        }
        */

        /// <summary>
        /// Get the corresponding source file path for a specific srcML file.
        /// For single folder storage algorithm
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public string GetSourcePathForXmlPath(string xmlPath)
        {
            string sourcePath = xmlPath.Substring(0, xmlPath.Length - 4);
            //sourcePath = Base32.FromBase32String(sourcePath);                     // Base32 decoding
            sourcePath = sourcePath.Replace("=", ":").Replace("-", "\\");           // Simple decoding
            //writeLog("D:\\Data\\log.txt", "GetSourcePathForXmlPath(): xmlPath = [" + xmlPath + "], sourcePath = [" + sourcePath + "]");
            return sourcePath;
        }

        /* // original implementation
        public string GetSourcePathForXmlPath(string xmlPath)
        {
            string fullPath = String.Empty;
            fullPath = (Path.IsPathRooted(xmlPath)) ? xmlPath : Path.GetFullPath(xmlPath);

            // ?? SourceDirectory and ArchivePath is not treated as the same type? (ISourceFolder sourceDirectory, string xmlDirectory)
            if (!fullPath.StartsWith(this.ArchivePath, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new IOException(String.Format("{0} is not rooted in {1}", xmlPath, this.ArchivePath));
            }

            string relativePath = fullPath.Substring(this.ArchivePath.Length + 1, fullPath.Length - this.ArchivePath.Length - 5);
            //Console.WriteLine("relativePath = " + relativePath);
            relativePath = Base32.FromBase32String(relativePath);
            //Console.WriteLine("relativePath decoded = " + relativePath);
            relativePath = relativePath.Substring(this.SourceDirectory.FullFolderPath.Length + 1);
            //Console.WriteLine("relativePath decoded = " + relativePath);
            string sourcePath = Path.Combine(this.SourceDirectory.FullFolderPath, relativePath);
            
            return sourcePath;
        }
        */

        /// <summary>
        /// Get the XElement for a specified source file.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        public XElement GetXElementForSourceFile(string sourceFilePath)
        {
            SrcMLFile srcMLFile = new SrcMLFile(sourceFilePath);
            XElement xElements = srcMLFile.FileUnits.FirstOrDefault();
            return xElements;
        }

        /// <summary>
        /// Raise a SrcML.NET event (SourceFileAdded, SourceFileChanged, SourceFileDeleted, SourceFileRenamed)
        /// </summary>
        /// <param name="e"></param>
        ////protected virtual void OnSourceFileChanged(SourceEventArgs e)
        protected virtual void OnSourceFileChanged(SrcMLDOTNETEventArgs e)
        {
            ////EventHandler<SourceEventArgs> handler = SourceFileChanged;
            EventHandler<SrcMLDOTNETEventArgs> handler = SrcMLDOTNETEventRaised;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raise a SrcML.NET event (StartupCompleted, MonitoringStopped, etc.)
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSrcMLDOTNETEventsRaised(SrcMLDOTNETEventArgs e)
        {
            EventHandler<SrcMLDOTNETEventArgs> handler = SrcMLDOTNETEventRaised;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        /// <summary>
        /// For debugging.
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="str"></param>
        private void writeLog(string logFile, string str)
        {
            StreamWriter sw = new StreamWriter(logFile, true, System.Text.Encoding.ASCII);
            sw.WriteLine(str);
            sw.Close();
        }

    }
}
