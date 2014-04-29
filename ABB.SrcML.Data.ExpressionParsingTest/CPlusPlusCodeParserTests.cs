﻿/******************************************************************************
 * Copyright (c) 2013 ABB Group
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *    Vinay Augustine (ABB Group) - initial API, implementation, & documentation
 *    Patrick Francis (ABB Group) - implementation and documentation
 *****************************************************************************/

using ABB.SrcML.Test.Utilities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Xml.Linq;

namespace ABB.SrcML.Data.Test {

    [TestFixture]
    [Category("Build")]
    public class CPlusPlusCodeParserTests {
        private AbstractCodeParser codeParser;
        private SrcMLFileUnitSetup fileSetup;

        [TestFixtureSetUp]
        public void ClassSetup() {
            codeParser = new CPlusPlusCodeParser();
            fileSetup = new SrcMLFileUnitSetup(Language.CPlusPlus);
        }

        [Test]
        public void TestCreateTypeDefinitions_Class() {
            // class A { };
            string xml = @"<class>class <name>A</name> <block>{<private type=""default""> </private>}</block>;</class>";
            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");

            var globalScope = codeParser.ParseFileUnit(xmlElement);
            var actual = globalScope.ChildStatements.First() as TypeDefinition;

            Assert.IsNotNull(actual);
            Assert.AreEqual("A", actual.Name);
            Assert.AreEqual(TypeKind.Class, actual.Kind);
            Assert.That(globalScope.IsGlobal);
            Assert.AreSame(globalScope, actual.ParentStatement);
        }

        [Test]
        public void TestCreateTypeDefinitions_ClassDeclaration() {
            //class A;
            string xml = @"<class_decl>class <name>A</name>;</class_decl>";
            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");

            var globalScope = codeParser.ParseFileUnit(xmlElement);
            var actual = globalScope.ChildStatements.First() as TypeDefinition;

            Assert.IsNotNull(actual);
            Assert.AreEqual("A", actual.Name);
            Assert.AreEqual(TypeKind.Class, actual.Kind);
            Assert.That(globalScope.IsGlobal);
            Assert.AreSame(globalScope, actual.ParentStatement);
        }

        [Test]
        public void TestClassWithDeclaredVariable() {
            //class A {
            //    int a;
            //};
            string xml = @"<class>class <name>A</name> <block>{<private type=""default"">
    <decl_stmt><decl><type><name>int</name></type> <name>a</name></decl>;</decl_stmt>
</private>}</block>;</class>";

            var globalScope = codeParser.ParseFileUnit(fileSetup.GetFileUnitForXmlSnippet(xml, "A.h"));

            

            //var classA = globalScope.ChildScopes.First() as ITypeDefinition;
            //Assert.AreEqual("A", classA.Name);
            //Assert.AreEqual(1, classA.DeclaredVariables.Count());
        }

        [Test]
        public void TestSimpleExpression() {
            //int foo = 42;
            string xml = @"<decl_stmt><decl><type><name>int</name></type> <name>foo</name> =<init> <expr><lit:literal type=""number"">42</lit:literal></expr></init></decl>;</decl_stmt>";

            var globalScope = codeParser.ParseFileUnit(fileSetup.GetFileUnitForXmlSnippet(xml, "A.cpp"));

        }

//        [Test]
//        public void TestConstructorWithCallToSelf() {
//            // test.h class MyClass {
//            // public:
//            // MyClass() : MyClass(0) { } MyClass(int foo) { } };
//            string xml = @"<class>class <name>MyClass</name> <block>{<private type=""default"">
//  </private><public>public:
//    <constructor><name>MyClass</name><parameter_list>()</parameter_list> <member_list>: <call><name>MyClass</name><argument_list>(<argument><expr><lit:literal type=""number"">0</lit:literal></expr></argument>)</argument_list></call> </member_list><block>{ }</block></constructor>
//    <constructor><name>MyClass</name><parameter_list>(<param><decl><type><name>int</name></type> <name>foo</name></decl></param>)</parameter_list> <block>{ }</block></constructor>
//</public>}</block>;</class>";
//            var unit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.h");
//            var globalScope = codeParser.ParseFileUnit(unit);

//            var constructors = globalScope.GetDescendantScopes<IMethodDefinition>();
//            var defaultConstructor = (from method in constructors
//                                      where method.Parameters.Count == 0
//                                      select method).FirstOrDefault();

//            var calledConstructor = (from method in constructors
//                                     where method.Parameters.Count == 1
//                                     select method).FirstOrDefault();

//            Assert.IsNotNull(defaultConstructor);
//            Assert.IsNotNull(calledConstructor);
//            Assert.AreEqual(1, defaultConstructor.MethodCalls.Count());

//            var constructorCall = defaultConstructor.MethodCalls.First();

//            Assert.AreSame(calledConstructor, constructorCall.FindMatches().FirstOrDefault());
//        }

//        [Test]
//        public void TestConstructorWithSuperClass() {
//            // test.h class SuperClass {
//            // public:
//            // SuperClass(int foo) { } }; class SubClass : public SuperClass {
//            // public:
//            // SubClass(int foo) : SuperClass(foo) { } };
//            string xml = @"<class>class <name>SuperClass</name> <block>{<private type=""default"">
//  </private><public>public:
//    <constructor><name>SuperClass</name><parameter_list>(<param><decl><type><name>int</name></type> <name>foo</name></decl></param>)</parameter_list> <block>{ }</block></constructor>
//</public>}</block>;</class>
//<class>class <name>SubClass</name> <super>: <specifier>public</specifier> <name>SuperClass</name></super> <block>{<private type=""default"">
//  </private><public>public:
//    <constructor><name>SubClass</name><parameter_list>(<param><decl><type><name>int</name></type> <name>foo</name></decl></param>)</parameter_list> <member_list>: <call><name>SuperClass</name><argument_list>(<argument><expr><name>foo</name></expr></argument>)</argument_list></call> </member_list><block>{ }</block></constructor>
//</public>}</block>;</class>";
//            var unit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.h");
//            var globalScope = codeParser.ParseFileUnit(unit);

//            var constructors = globalScope.GetDescendantScopes<IMethodDefinition>();
//            var subClassConstructor = (from method in constructors
//                                       where method.GetParentScopes<ITypeDefinition>().First().Name == "SubClass"
//                                       select method).FirstOrDefault();

//            var calledConstructor = (from method in constructors
//                                     where method.GetParentScopes<ITypeDefinition>().First().Name == "SuperClass"
//                                     select method).FirstOrDefault();

//            Assert.IsNotNull(subClassConstructor);
//            Assert.IsNotNull(calledConstructor);
//            Assert.AreEqual(1, subClassConstructor.MethodCalls.Count());

//            var constructorCall = subClassConstructor.MethodCalls.First();

//            Assert.AreSame(calledConstructor, constructorCall.FindMatches().FirstOrDefault());
//        }

//        [Test]
//        public void TestCreateAliasesForFiles_ImportClass() {
//            // using A::Foo;
//            string xml = @"<using>using <name><name>A</name><op:operator>::</op:operator><name>Foo</name></name>;</using>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.cpp");

//            var actual = codeParser.ParseAliasElement(xmlElement.Element(SRC.Using), new ParserContext(xmlElement));

//            Assert.AreEqual("Foo", actual.ImportedNamedScope.Name);
//            Assert.AreEqual("A", actual.ImportedNamespace.Name);
//            Assert.IsFalse(actual.IsNamespaceImport);
//        }

//        [Test]
//        public void TestCreateAliasesForFiles_ImportNamespace() {
//            // using namespace x::y::z;
//            string xml = @"<using>using namespace <name><name>x</name><op:operator>::</op:operator><name>y</name><op:operator>::</op:operator><name>z</name></name>;</using>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.cpp");

//            var actual = codeParser.ParseAliasElement(xmlElement.Element(SRC.Using), new ParserContext(xmlElement));

//            Assert.IsNull(actual.ImportedNamedScope);
//            Assert.That(actual.IsNamespaceImport);
//            Assert.AreEqual("x", actual.ImportedNamespace.Name);
//            Assert.AreEqual("y", actual.ImportedNamespace.ChildScopeUse.Name);
//            Assert.AreEqual("z", actual.ImportedNamespace.ChildScopeUse.ChildScopeUse.Name);
//        }

//        [Test]
//        public void TestCreateTypeDefinition_ClassInNamespace() {
//            // namespace A { class B { }; }
//            string xml = @"<namespace>namespace <name>A</name> <block>{
//    <class>class <name>B</name> <block>{<private type=""default"">
//    </private>}</block>;</class>
//}</block></namespace>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "B.h");

//            var globalScope = codeParser.ParseFileUnit(xmlElement);
//            var namespaceA = globalScope.ChildScopes.First() as INamespaceDefinition;
//            var typeB = namespaceA.ChildScopes.First() as ITypeDefinition;

//            Assert.AreEqual("A", namespaceA.Name);
//            Assert.IsFalse(namespaceA.IsGlobal);

//            Assert.AreEqual("B", typeB.Name);
//        }

//        [Test]
//        public void TestCreateTypeDefinition_ClassWithMethodDeclaration() {
//            // class A {
//            // public:
//            // int foo(int a); };
//            string xml = @"<class>class <name>A</name> <block>{<private type=""default"">
//</private><public>public:
//    <function_decl><type><name>int</name></type> <name>foo</name><parameter_list>(<param><decl><type><name>int</name></type> <name>a</name></decl></param>)</parameter_list>;</function_decl>
//</public>}</block>;</class>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");

//            var globalScope = codeParser.ParseFileUnit(xmlElement);
//            var scopes = VariableScopeIterator.Visit(globalScope);

//            var typeA = globalScope.ChildScopes.First() as ITypeDefinition;
//            var methodFoo = typeA.ChildScopes.First() as IMethodDefinition;
//            Assert.AreEqual(3, scopes.Count());

//            Assert.AreEqual("A", typeA.Name);
//            Assert.AreEqual("foo", methodFoo.Name);

//            Assert.AreEqual(1, methodFoo.Parameters.Count);
//        }

//        [Test]
//        [Category("Todo")]
//        public void TestCreateTypeDefinition_StaticMethod() {
//            //class Example {
//            //public:
//            //    static int Example::Foo(int bar) { return bar+1; }
//            //};
//            string xml = @"<class>class <name>Example</name> <block>{<private type=""default"">
//</private><public>public:
//    <function><type><name>static</name> <name>int</name></type> <name><name>Example</name><op:operator>::</op:operator><name>Foo</name></name><parameter_list>(<param><decl><type><name>int</name></type> <name>bar</name></decl></param>)</parameter_list> <block>{ <return>return <expr><name>bar</name><op:operator>+</op:operator><lit:literal type=""number"">1</lit:literal></expr>;</return> }</block></function>
//</public>}</block>;</class>";
//            var fileUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "static_method.h");
//            var globalScope = codeParser.ParseFileUnit(fileUnit);

//            var example = globalScope.ChildScopes.OfType<ITypeDefinition>().FirstOrDefault();
//            Assert.IsNotNull(example);
//            Assert.AreEqual("Example", example.Name);
//            Assert.AreEqual(1, example.ChildScopes.Count());
//            var foo = example.ChildScopes.OfType<IMethodDefinition>().FirstOrDefault();
//            Assert.IsNotNull(foo, "TODO fix static methods");
//            Assert.AreEqual("Foo", foo.Name);
//        }

//        

//        [Test]
//        public void TestCreateTypeDefinitions_ClassInFunction() {
//            // int main() { class A { }; }
//            string xml = @"<function><type><name>int</name></type> <name>main</name><parameter_list>()</parameter_list> <block>{
//    <class>class <name>A</name> <block>{<private type=""default"">
//    </private>}</block>;</class>
//}</block></function>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "main.cpp");
//            var mainMethod = codeParser.ParseFileUnit(xmlElement).ChildScopes.First() as IMethodDefinition;

//            Assert.AreEqual("main", mainMethod.Name);

//            var typeA = mainMethod.ChildScopes.First() as ITypeDefinition;
//            Assert.AreEqual("A", typeA.Name);
//            Assert.AreEqual("main.A", typeA.GetFullName());
//            Assert.AreEqual(String.Empty, typeA.GetFirstParent<INamespaceDefinition>().GetFullName());
//        }

//        [Test]
//        public void TestCreateTypeDefinitions_ClassWithInnerClass() {
//            // class A { class B { }; };
//            string xml = @"<class>class <name>A</name> <block>{<private type=""default"">
//    <class>class <name>B</name> <block>{<private type=""default"">
//    </private>}</block>;</class>
//</private>}</block>;</class>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");
//            var globalScope = codeParser.ParseFileUnit(xmlElement);

//            var typeA = globalScope.ChildScopes.First() as ITypeDefinition;
//            var typeB = typeA.ChildScopes.First() as ITypeDefinition;

//            Assert.AreSame(typeA, typeB.ParentScope);
//            Assert.AreEqual("A", typeA.GetFullName());

//            Assert.AreEqual("A.B", typeB.GetFullName());
//        }

//        [Test]
//        public void TestCreateTypeDefinitions_ClassWithParents() {
//            // class A : B,C,D { };
//            string xml = @"<class>class <name>A</name> <super>: <name>B</name>,<name>C</name>,<name>D</name></super> <block>{<private type=""default"">
//</private>}</block>;</class>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");
//            var globalScope = codeParser.ParseFileUnit(xmlElement);
//            var actual = globalScope.ChildScopes.First() as ITypeDefinition;

//            Assert.AreEqual("A", actual.Name);
//            Assert.AreEqual(3, actual.ParentTypes.Count);
//            Assert.That(globalScope.IsGlobal);
//            Assert.AreSame(globalScope, actual.ParentScope);

//            var parentNames = from parent in actual.ParentTypes
//                              select parent.Name;

//            var tests = Enumerable.Zip<string, string, bool>(new[] { "B", "C", "D" }, parentNames, (e, a) => e == a
//                );
//            foreach(var parentMatchesExpected in tests) {
//                Assert.That(parentMatchesExpected);
//            }
//        }

//        [Test]
//        public void TestCreateTypeDefinitions_ClassWithQualifiedParent() {
//            // class D : A::B::C { }
//            string xml = @"<class>class <name>D</name> <super>: <name><name>A</name><op:operator>::</op:operator><name>B</name><op:operator>::</op:operator><name>C</name></name></super> <block>{<private type=""default"">
//</private>}</block>;</class>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "D.h");
//            var actual = codeParser.ParseFileUnit(xmlElement).ChildScopes.First() as ITypeDefinition;
//            var globalNamespace = actual.ParentScope as INamespaceDefinition;

//            Assert.AreEqual("D", actual.Name);
//            Assert.AreEqual(1, actual.ParentTypes.Count);
//            Assert.That(globalNamespace.IsGlobal);

//            var parent = actual.ParentTypes.First();

//            Assert.AreEqual("C", parent.Name);
//            TestHelper.VerifyPrefixValues(new[] { "A", "B" }, parent.Prefix);
//        }

//        [Test]
//        public void TestCreateTypeDefinitions_InnerClassWithNamespace() {
//            // namespace A { class B { class C { }; }; }
//            string xml = @"<namespace>namespace <name>A</name> <block>{
//    <class>class <name>B</name> <block>{<private type=""default"">
//        <class>class <name>C</name> <block>{<private type=""default"">
//        </private>}</block>;</class>
//    </private>}</block>;</class>
//}</block></namespace>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");
//            var scopes = VariableScopeIterator.Visit(codeParser.ParseFileUnit(xmlElement));

//            Assert.AreEqual(4, scopes.Count());

//            var typeDefinitions = from scope in scopes
//                                  let definition = scope as ITypeDefinition
//                                  where definition != null
//                                  select definition;

//            var outer = typeDefinitions.First() as ITypeDefinition;
//            var inner = typeDefinitions.Last() as ITypeDefinition;

//            Assert.AreEqual("B", outer.Name);
//            Assert.AreEqual("A", outer.GetFirstParent<INamespaceDefinition>().GetFullName());
//            Assert.AreEqual("A.B", outer.GetFullName());

//            Assert.AreEqual("C", inner.Name);
//            Assert.AreEqual("A", inner.GetFirstParent<INamespaceDefinition>().GetFullName());
//            Assert.AreEqual("A.B.C", inner.GetFullName());
//        }

//        [Test]
//        public void TestCreateTypeDefinitions_Struct() {
//            // struct A { };
//            string xml = @"<struct>struct <name>A</name> <block>{<public type=""default"">
//</public>}</block>;</struct>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");
//            var actual = codeParser.ParseFileUnit(xmlElement).ChildScopes.First() as ITypeDefinition;
//            var globalNamespace = actual.ParentScope as INamespaceDefinition;

//            Assert.AreEqual("A", actual.Name);
//            Assert.AreEqual(TypeKind.Struct, actual.Kind);
//            Assert.That(globalNamespace.IsGlobal);
//        }

//        [Test]
//        public void TestCreateTypeDefinitions_Union() {
//            // union A { int a; char b;
//            //};
//            string xml = @"<union>union <name>A</name> <block>{<public type=""default"">
//    <decl_stmt><decl><type><name>int</name></type> <name>a</name></decl>;</decl_stmt>
//    <decl_stmt><decl><type><name>char</name></type> <name>b</name></decl>;</decl_stmt>
//</public>}</block>;</union>";

//            XElement xmlElement = fileSetup.GetFileUnitForXmlSnippet(xml, "A.h");
//            var actual = codeParser.ParseFileUnit(xmlElement).ChildScopes.First() as ITypeDefinition;
//            var globalNamespace = actual.ParentScope as INamespaceDefinition;
//            Assert.AreEqual(TypeKind.Union, actual.Kind);
//            Assert.That(globalNamespace.IsGlobal);
//        }

//        [Test]
//        public void TestGenericVariableDeclaration() {
//            //vector<int> a;
//            string xml = @"<decl_stmt><decl><type><name><name>vector</name><argument_list>&lt;<argument><name>int</name></argument>&gt;</argument_list></name></type> <name>a</name></decl>;</decl_stmt>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");

//            var testScope = codeParser.ParseFileUnit(testUnit);

//            var testDeclaration = testScope.DeclaredVariables.First();
//            Assert.IsNotNull(testDeclaration, "could not find the test declaration");
//            Assert.AreEqual("a", testDeclaration.Name);
//            Assert.AreEqual("vector", testDeclaration.VariableType.Name);
//            Assert.That(testDeclaration.VariableType.IsGeneric);
//            Assert.AreEqual(1, testDeclaration.VariableType.TypeParameters.Count);
//            Assert.AreEqual("int", testDeclaration.VariableType.TypeParameters.First().Name);
//        }

//        [Test]
//        public void TestGenericVariableDeclarationWithPrefix() {
//            //std::vector<int> a;
//            string xml = @"<decl_stmt><decl><type><name><name>std</name><op:operator>::</op:operator><name><name>vector</name><argument_list>&lt;<argument><name>int</name></argument>&gt;</argument_list></name></name></type> <name>a</name></decl>;</decl_stmt>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");

//            var testScope = codeParser.ParseFileUnit(testUnit);

//            var testDeclaration = testScope.DeclaredVariables.First();
//            Assert.IsNotNull(testDeclaration, "could not find the test declaration");
//            Assert.AreEqual("a", testDeclaration.Name);
//            Assert.AreEqual("vector", testDeclaration.VariableType.Name);
//            Assert.AreEqual("std", testDeclaration.VariableType.Prefix.Name);
//            Assert.That(testDeclaration.VariableType.IsGeneric);
//            Assert.AreEqual(1, testDeclaration.VariableType.TypeParameters.Count);
//            Assert.AreEqual("int", testDeclaration.VariableType.TypeParameters.First().Name);
//        }

//        [Test]
//        public void TestLengthyCallingObjectChain() {
//            //a->b.Foo();
//            string xml = @"<expr_stmt><expr><name>a</name><op:operator>-&gt;</op:operator><name>b</name><op:operator>.</op:operator><call><name>Foo</name><argument_list>()</argument_list></call></expr>;</expr_stmt>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");

//            var testScope = codeParser.ParseFileUnit(testUnit);

//            var testCall = testScope.MethodCalls.FirstOrDefault();
//            Assert.IsNotNull(testCall, "could not find any test calls");
//            Assert.AreEqual("Foo", testCall.Name);
//            Assert.AreEqual("b", (testCall.CallingObject as IVariableUse).Name);
//            Assert.AreEqual("a", (testCall.CallingObject.CallingObject as IVariableUse).Name);
//            Assert.IsNull(testCall.CallingObject.CallingObject.CallingObject);
//        }

//        [Test]
//        [Category("Todo")]
//        public void TestMergeWithUsing() {
//            // namespace A { class B { void Foo(); }; }
//            string headerXml = @"<namespace>namespace <name>A</name> <block>{ <class>class <name>B</name> <block>{<private type=""default""> <function_decl><type><name>void</name></type> <name>Foo</name><parameter_list>()</parameter_list>;</function_decl> </private>}</block>;</class> }</block></namespace>";

//            //using namespace A;
//            //
//            //void B::Foo() { }
//            string implementationXml = @"<using>using namespace <name>A</name>;</using>
//
//<function><type><name>void</name></type> <name><name>B</name><op:operator>::</op:operator><name>Foo</name></name><parameter_list>()</parameter_list> <block>{ }</block></function>";

//            var headerScope = codeParser.ParseFileUnit(fileSetup.GetFileUnitForXmlSnippet(headerXml, "A.h"));
//            var implementationScope = codeParser.ParseFileUnit(fileSetup.GetFileUnitForXmlSnippet(implementationXml, "A.cpp"));

//            var globalScope = headerScope.Merge(implementationScope);

//            Assert.AreEqual(1, globalScope.ChildScopes.Count(), "TODO implement using statements in C++");

//            var namespaceA = globalScope.ChildScopes.First() as INamespaceDefinition;
//            Assert.AreEqual("A", namespaceA.Name);
//            Assert.AreEqual(1, namespaceA.ChildScopes.Count());

//            var typeB = namespaceA.ChildScopes.First() as ITypeDefinition;
//            Assert.AreEqual("B", typeB.Name);
//            Assert.AreEqual(1, typeB.ChildScopes.Count());

//            var methodFoo = typeB.ChildScopes.First() as IMethodDefinition;
//            Assert.AreEqual("Foo", methodFoo);
//            Assert.IsEmpty(typeB.ChildScopes);

//            var globalScope_implementationFirst = implementationScope.Merge(headerScope);

//            Assert.IsTrue(TestHelper.ScopesAreEqual(globalScope, globalScope_implementationFirst));
//        }

//        [Test]
//        public void TestMethodCallCreation_WithConflictingMethodNames() {
//            //# A.h
//            //class A {
//            //    B b;
//            //public:
//            //    bool Contains() { b.Contains(); }
//            //};
//            string a_xml = @"<class>class <name>A</name> <block>{<private type=""default"">
//    <decl_stmt><decl><type><name>B</name></type> <name>b</name></decl>;</decl_stmt>
//</private><public>public:
//    <function><type><name>bool</name></type> <name>Contains</name><parameter_list>()</parameter_list> <block>{ <expr_stmt><expr><name>b</name><op:operator>.</op:operator><call><name>Contains</name><argument_list>()</argument_list></call></expr>;</expr_stmt> }</block></function>
//</public>}</block>;</class>";

//            //# B.h
//            //class B {
//            //public:
//            //    bool Contains() { return true; }
//            //};
//            string b_xml = @"<class>class <name>B</name> <block>{<private type=""default"">
//</private><public>public:
//    <function><type><name>bool</name></type> <name>Contains</name><parameter_list>()</parameter_list> <block>{ <return>return <expr><lit:literal type=""boolean"">true</lit:literal></expr>;</return> }</block></function>
//</public>}</block>;</class>";

//            var fileUnitA = fileSetup.GetFileUnitForXmlSnippet(a_xml, "A.h");
//            var fileUnitB = fileSetup.GetFileUnitForXmlSnippet(b_xml, "B.h");

//            var scopeForA = codeParser.ParseFileUnit(fileUnitA);
//            var scopeForB = codeParser.ParseFileUnit(fileUnitB);
//            var globalScope = scopeForA.Merge(scopeForB);

//            var classA = globalScope.ChildScopes.First() as ITypeDefinition;
//            var classB = globalScope.ChildScopes.Last() as ITypeDefinition;
//            Assert.AreEqual("A", classA.Name);
//            Assert.AreEqual("B", classB.Name);

//            var aDotContains = classA.ChildScopes.First() as IMethodDefinition;
//            var bDotContains = classB.ChildScopes.First() as IMethodDefinition;

//            Assert.AreEqual("A.Contains", aDotContains.GetFullName());
//            Assert.AreEqual("B.Contains", bDotContains.GetFullName());

//            var methodCall = aDotContains.MethodCalls.First();
//            var variableB = classA.DeclaredVariables.First();

//            Assert.AreEqual("b", (methodCall.CallingObject as IVariableUse).Name);
//            Assert.AreEqual("b", variableB.Name);
//            Assert.AreSame(variableB, (methodCall.CallingObject as IVariableUse).FindMatches().First());

//            Assert.AreSame(bDotContains, methodCall.FindMatches().First());
//            Assert.AreNotSame(aDotContains, methodCall.FindMatches().First());
//        }

//        [Test]
//        public void TestMethodCallCreation_WithThisKeyword() {
//            //class A {
//            //    void Bar() { }
//            //    class B {
//            //        int a;
//            //        void Foo() { this->Bar(); }
//            //        void Bar() { return this->a; }
//            //    };
//            //};
//            string a_xml = @"<class>class <name>A</name> <block>{<private type=""default"">
//    <function><type><name>void</name></type> <name>Bar</name><parameter_list>()</parameter_list> <block>{ }</block></function>
//    <class>class <name>B</name> <block>{<private type=""default"">
//        <decl_stmt><decl><type><name>int</name></type> <name>a</name></decl>;</decl_stmt>
//        <function><type><name>void</name></type> <name>Foo</name><parameter_list>()</parameter_list> <block>{ <return>return <expr><name>this</name><op:operator>-&gt;</op:operator><call><name>Bar</name><argument_list>()</argument_list></call></expr>;</return> }</block></function>
//        <function><type><name>void</name></type> <name>Bar</name><parameter_list>()</parameter_list> <block>{ <return>return <expr><name>this</name><op:operator>-&gt;</op:operator><name>a</name></expr>;</return> }</block></function>
//    </private>}</block>;</class>
//</private>}</block>;</class>";

//            var fileUnit = fileSetup.GetFileUnitForXmlSnippet(a_xml, "A.java");
//            var globalScope = codeParser.ParseFileUnit(fileUnit);

//            var aDotBar = globalScope.ChildScopes.First().ChildScopes.First() as IMethodDefinition;
//            var aDotBDotFoo = globalScope.ChildScopes.First().ChildScopes.Last().ChildScopes.First() as IMethodDefinition;
//            var aDotBDotBar = globalScope.ChildScopes.First().ChildScopes.Last().ChildScopes.Last() as IMethodDefinition;

//            Assert.AreEqual("A.Bar", aDotBar.GetFullName());
//            Assert.AreEqual("A.B.Foo", aDotBDotFoo.GetFullName());
//            Assert.AreEqual("A.B.Bar", aDotBDotBar.GetFullName());

//            Assert.AreSame(aDotBDotBar, aDotBDotFoo.MethodCalls.First().FindMatches().First());
//            Assert.AreEqual(1, aDotBDotFoo.MethodCalls.First().FindMatches().Count());
//        }

//        [Test]
//        public void TestMethodDefinitionWithReturnType() {
//            //int Foo() { }
//            string xml = @"<function><type><name>int</name></type> <name>Foo</name><parameter_list>()</parameter_list> <block>{ }</block></function>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");

//            var testScope = codeParser.ParseFileUnit(testUnit);

//            var method = testScope.GetChildScopesWithId<IMethodDefinition>("Foo").FirstOrDefault();
//            Assert.IsNotNull(method, "could not find the test method");

//            Assert.AreEqual("int", method.ReturnType.Name);
//            Assert.AreEqual("Method: int Foo()", method.ToString());
//        }

//        [Test]
//        public void TestMethodDefinitionWithReturnTypeAndWithSpecifier() {
//            //static int Foo() { }
//            string xml = @"<function><type><name>static</name> <name>int</name></type> <name>Foo</name><parameter_list>()</parameter_list> <block>{ }</block></function>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");

//            var testScope = codeParser.ParseFileUnit(testUnit);

//            var method = testScope.GetChildScopesWithId<IMethodDefinition>("Foo").FirstOrDefault();
//            Assert.IsNotNull(method, "could not find the test method");

//            Assert.AreEqual("int", method.ReturnType.Name);
//        }

//        [Test]
//        public void TestMethodDefinitionWithVoidParameter() {
//            //void Foo(void) { }
//            string xml = @"<function><type><name>void</name></type> <name>Foo</name><parameter_list>(<param><decl><type><name>void</name></type></decl></param>)</parameter_list> <block>{ }</block></function>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");

//            var testScope = codeParser.ParseFileUnit(testUnit);

//            var method = testScope.GetChildScopesWithId<IMethodDefinition>("Foo").FirstOrDefault();
//            Assert.IsNotNull(method, "could not find the test method");

//            Assert.AreEqual(0, method.Parameters.Count);
//        }

//        [Test]
//        public void TestMethodDefinitionWithVoidReturn() {
//            //void Foo() { }
//            string xml = @"<function><type><name>void</name></type> <name>Foo</name><parameter_list>()</parameter_list> <block>{ }</block></function>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");

//            var testScope = codeParser.ParseFileUnit(testUnit);

//            var method = testScope.GetChildScopesWithId<IMethodDefinition>("Foo").FirstOrDefault();
//            Assert.IsNotNull(method, "could not find the test method");

//            Assert.IsNull(method.ReturnType, "return type should be null");
//            Assert.AreEqual("Method: void Foo()", method.ToString());
//        }

//        [Test]
//        public void TestMethodWithDefaultArguments() {
//            //void foo(int a = 0);
//            //
//            //int main() {
//            //    foo();
//            //    foo(5);
//            //    return 0;
//            //}
//            //
//            //void foo(int a) { }
//            string xml = @"<function_decl><type><name>void</name></type> <name>foo</name><parameter_list>(<param><decl><type><name>int</name></type> <name>a</name> =<init> <expr><lit:literal type=""number"">0</lit:literal></expr></init></decl></param>)</parameter_list>;</function_decl>
//
//<function><type><name>int</name></type> <name>main</name><parameter_list>()</parameter_list> <block>{
//    <expr_stmt><expr><call><name>foo</name><argument_list>()</argument_list></call></expr>;</expr_stmt>
//    <expr_stmt><expr><call><name>foo</name><argument_list>(<argument><expr><lit:literal type=""number"">5</lit:literal></expr></argument>)</argument_list></call></expr>;</expr_stmt>
//    <return>return <expr><lit:literal type=""number"">0</lit:literal></expr>;</return>
//}</block></function>
//
//<function><type><name>void</name></type> <name>foo</name><parameter_list>(<param><decl><type><name>int</name></type> <name>a</name></decl></param>)</parameter_list> <block>{ }</block></function>";

//            var unit = fileSetup.GetFileUnitForXmlSnippet(xml, "test.cpp");
//            var globalScope = codeParser.ParseFileUnit(unit);

//            var fooMethod = globalScope.GetChildScopesWithId<IMethodDefinition>("foo").FirstOrDefault();
//            var mainMethod = globalScope.GetChildScopesWithId<IMethodDefinition>("main").FirstOrDefault();

//            Assert.IsNotNull(fooMethod);
//            Assert.IsNotNull(mainMethod);
//            Assert.AreEqual(2, mainMethod.MethodCalls.Count());

//            Assert.AreSame(fooMethod, mainMethod.MethodCalls.First().FindMatches().FirstOrDefault());
//            Assert.AreSame(fooMethod, mainMethod.MethodCalls.Last().FindMatches().FirstOrDefault());
//        }

//        [Test]
//        public void TestMultiVariableDeclarations() {
//            //int a,b,c;
//            string testXml = @"<decl_stmt><decl><type><name>int</name></type> <name>a</name>,<name>b</name>,<name>c</name></decl>;</decl_stmt>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(testXml, "test.cpp");

//            var globalScope = codeParser.ParseFileUnit(testUnit);

//            Assert.AreEqual(3, globalScope.DeclaredVariables.Count());

//            var declaredVariableNames = from variable in globalScope.DeclaredVariables select variable.Name;
//            var expectedVariableNames = new string[] { "a", "b", "c" };

//            CollectionAssert.AreEquivalent(expectedVariableNames, declaredVariableNames);
//        }

//        [Test]
//        public void TestVariablesWithSpecifiers() {
//            //const int A;
//            //static int B;
//            //static const Foo C;
//            //extern Foo D;
//            string testXml = @"<decl_stmt><decl><type><name>const</name> <name>int</name></type> <name>A</name></decl>;</decl_stmt>
//<decl_stmt><decl><type><name>static</name> <name>int</name></type> <name>B</name></decl>;</decl_stmt>
//<decl_stmt><decl><type><name>static</name> <name>const</name> <name>Foo</name></type> <name>C</name></decl>;</decl_stmt>
//<decl_stmt><decl><type><name>extern</name> <name>Foo</name></type> <name>D</name></decl>;</decl_stmt>";

//            var testUnit = fileSetup.GetFileUnitForXmlSnippet(testXml, "test.cpp");

//            var globalScope = codeParser.ParseFileUnit(testUnit);

//            var declaredVariableNames = from variable in globalScope.DeclaredVariables select variable.Name;
//            var declaredVariableTypes = from variable in globalScope.DeclaredVariables select variable.VariableType.Name;

//            var expectedVariableNames = new string[] { "A", "B", "C", "D" };
//            var expectedVariableTypes = new string[] { "int", "Foo" };

//            CollectionAssert.AreEquivalent(expectedVariableNames, declaredVariableNames);
//            foreach(var declaration in globalScope.DeclaredVariables) {
//                CollectionAssert.Contains(expectedVariableTypes, declaration.VariableType.Name);
//            }
//        }
    }
}