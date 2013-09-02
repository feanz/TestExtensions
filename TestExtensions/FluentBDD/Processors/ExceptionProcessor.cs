using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestExtensions.FluentBDD.Processors
{
	public class ExceptionProcessor : IProcessor
	{
		private static readonly Action BestGuessInconclusiveAssertion;

		private static readonly List<string> ExcludedAssemblies =
			new List<string>(new[] {"System", "mscorlib", "TestExtensions", "TestDriven", "JetBrains.ReSharper"});

		private readonly Action _assertInconclusive;

		static ExceptionProcessor()
		{
			var exceptionType = typeof (Exception);
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (ExcludedAssemblies.Any(ex => assembly.GetName().FullName.StartsWith(ex)))
					continue;

				foreach (var inconclusiveExceptionType in GetTypesSafely(assembly))
				{
					if (inconclusiveExceptionType.Name.Contains("Inconclusive") &&
					    inconclusiveExceptionType.Name.Contains("Exception") &&
					    exceptionType.IsAssignableFrom(inconclusiveExceptionType))
					{
						var constructors = inconclusiveExceptionType.GetConstructors();
						var shortestCtor = constructors.Min(c => c.GetParameters().Length);
						var ctor = constructors.First(c => c.GetParameters().Length == shortestCtor);
						var argList = new List<object>();
						argList.AddRange(ctor.GetParameters().Select(p => DefaultValue(p.ParameterType)));
						BestGuessInconclusiveAssertion = () => { throw (Exception) ctor.Invoke(argList.ToArray()); };
						return;
					}
				}
			}

			BestGuessInconclusiveAssertion = () => { throw new InconclusiveException(); };
		}

		public ExceptionProcessor(Action assertInconclusive)
		{
			_assertInconclusive = assertInconclusive;
		}

		public ExceptionProcessor()
			: this(BestGuessInconclusiveAssertion)
		{
		}

		public void Process(Story story)
		{
			var allSteps = story.Scenarios.SelectMany(s => s.Steps).ToList();
			if (!allSteps.Any())
				return;

			var worseResult = story.Result;

			var stepWithWorseResult = allSteps.First(s => s.Result == worseResult);

			if (worseResult == StepExecutionResult.Failed || worseResult == StepExecutionResult.Inconclusive)
			{
				PreserveStackTrace(stepWithWorseResult.Exception);
				throw stepWithWorseResult.Exception;
			}

			if (worseResult == StepExecutionResult.NotImplemented)
				_assertInconclusive();
		}

		public ProcessType ProcessType
		{
			get { return ProcessType.Execute; }
		}

		private static object DefaultValue(Type myType)
		{
			return !myType.IsValueType ? null : Activator.CreateInstance(myType);
		}

		private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				return ex.Types.Where(x => x != null);
			}
		}

		private static void PreserveStackTrace(Exception exception)
		{
			var preserveStackTrace = typeof (Exception).GetMethod("InternalPreserveStackTrace",
				BindingFlags.Instance | BindingFlags.NonPublic);
			preserveStackTrace.Invoke(exception, null);
		}
	}
}