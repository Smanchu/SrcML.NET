﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABB.SrcML.Data.Queries {
    public class FindMethodCallsAtLocationQuery : AbstractQuery<SourceLocation, Collection<IMethodCall>> {
        public FindMethodCallsAtLocationQuery(IDataRepository data, int lockTimeout, TaskFactory factory)
            : base(data, lockTimeout, factory) { }

        public override Collection<IMethodCall> Execute(IScope globalScope, SourceLocation parameter) {
            if(globalScope != null) {
                var scope = globalScope.GetScopeForLocation(parameter);
                if(scope != null) {
                    var calls = scope.MethodCalls.Where(mc => mc.Location.Contains(parameter));
                    return new Collection<IMethodCall>(calls.OrderByDescending(mc => mc.Location, new SourceLocationComparer()).ToList());
                }
            }
            return new Collection<IMethodCall>();
        }

        private class SourceLocationComparer : Comparer<SourceLocation> {

            public override int Compare(SourceLocation x, SourceLocation y) {
                if(object.Equals(x, y))
                    return 0;
                if(x == null && y != null)
                    return -1;
                if(x != null && y == null)
                    return 1;

                var result = x.StartingLineNumber.CompareTo(y.StartingLineNumber);
                if(result == 0) {
                    result = x.StartingColumnNumber.CompareTo(y.StartingColumnNumber);
                }
                return result;
            }
        }
    }
}
