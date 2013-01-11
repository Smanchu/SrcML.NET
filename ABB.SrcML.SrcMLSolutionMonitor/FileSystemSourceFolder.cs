﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ABB.SrcML.SrcMLSolutionMonitor
{
    public class FileSystemSourceFolder : ISourceFolder
    {
        private DirectoryInfo _folderInfo;
        private FileSystemWatcher _directoryWatcher;

        protected virtual void OnSourceFileChanged(SourceEventArgs e)
        {
            writeLog("D:\\Data\\log.txt", "OnSourceFileChanged()");
            EventHandler<SourceEventArgs> handler = SourceFileChanged;

            if (null != handler)
            {
                writeLog("D:\\Data\\log.txt", "handler(this, e)");
                handler(this, e);
            }
        }

        public FileSystemSourceFolder(string pathToSourceFolder)
        {
            writeLog("D:\\Data\\log.txt", "******* FileSystemSourceFolder(): srcMLFolder = [" + pathToSourceFolder + "]");
            this.FullFolderPath = pathToSourceFolder;
            SetupFileSystemWatcher();
            StartWatching();
        }
        #region ISourceFolder Members

        public event EventHandler<SourceEventArgs> SourceFileChanged;

        public string FullFolderPath
        {
            get
            {
                return this._folderInfo.FullName;
            }
            set
            {
                this._folderInfo = new DirectoryInfo(value);
            }
        }

        public void StartWatching()
        {
            writeLog("D:\\Data\\log.txt", "StartWatching()");
            this._directoryWatcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            writeLog("D:\\Data\\log.txt", "StopWatching()");
            this._directoryWatcher.EnableRaisingEvents = false;
        }

        #endregion

        private void SetupFileSystemWatcher()
        {
            writeLog("D:\\Data\\log.txt", "SetupFileSystemWatcher(): srcMLFolder = [" + this.FullFolderPath + "]");
            this._directoryWatcher = new FileSystemWatcher(this.FullFolderPath);

            this._directoryWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Attributes;
            this._directoryWatcher.IncludeSubdirectories = true;

            this._directoryWatcher.Changed += HandleFileChanged;
            this._directoryWatcher.Created += HandleFileCreated;
            this._directoryWatcher.Deleted += HandleFileDeleted;
            this._directoryWatcher.Error += HandleFileWatcherError;
            this._directoryWatcher.Renamed += HandleFileRenamed;
        }

        void HandleFileChanged(object sender, FileSystemEventArgs e)
        {
            writeLog("D:\\Data\\log.txt", "HandleFileChanged(): e.FullPath = [" + e.FullPath + "]");
            handleFileEvent(e.FullPath, e.FullPath, SourceEventType.Changed);
        }

        // When created a file, HandleFileChanged is also triggered
        void HandleFileCreated(object sender, FileSystemEventArgs e)
        {
            writeLog("D:\\Data\\log.txt", "HandleFileCreated(): e.FullPath = [" + e.FullPath + "]");
            handleFileEvent(e.FullPath, e.FullPath, SourceEventType.Added);
        }

        void HandleFileDeleted(object sender, FileSystemEventArgs e)
        {
            writeLog("D:\\Data\\log.txt", "HandleFileDeleted(): e.FullPath = [" + e.FullPath + "]");
            handleFileEvent(e.FullPath, e.FullPath, SourceEventType.Deleted);
        }

        void HandleFileWatcherError(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void HandleFileRenamed(object sender, RenamedEventArgs e)
        {
            writeLog("D:\\Data\\log.txt", "HandleFileRenamed(): e.FullPath = [" + e.FullPath + "], e.OldFullPath = [" + e.OldFullPath + "]");
            handleFileEvent(e.FullPath, e.OldFullPath, SourceEventType.Renamed);
        }

        private void handleFileEvent(string pathToFile, string oldPathToFile, SourceEventType eventType)
        {
            writeLog("D:\\Data\\log.txt", "handleFileEvent(): pathToFile = [" + pathToFile + "], oldPathToFile = [" + oldPathToFile + "]");
            if (isFile(pathToFile))
            {
                writeLog("D:\\Data\\log.txt", "isFile(): pathToFile = [" + pathToFile + "], oldPathToFile = [" + oldPathToFile + "]");
                OnSourceFileChanged(new SourceEventArgs(pathToFile, oldPathToFile, eventType));
            }
        }

        private static bool isFile(string fullPath)
        {
            if (!File.Exists(fullPath))
                return false;

            var pathAttributes = File.GetAttributes(fullPath);
            return !pathAttributes.HasFlag(FileAttributes.Directory);
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
