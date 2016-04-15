namespace NxBRE.FlowEngine.Core
{
	using System;
	using System.Text;
	using System.Collections;
	
	/// <summary> Abstract implementation of BRERuleMetaData.
	/// Use this if you wish to do inheritance.
	/// *
	/// </summary>
	/// <seealso cref="NxBRE.FlowEngine.IBRERuleMetaData"/>
	/// <author>  Sloan Seaman
	/// </author>
	internal abstract class AbstractBRERuleMetaData : IBRERuleMetaData
	{
		/// <summary> Returns the BRERuleFactory that created the Business Rule
		/// *
		/// </summary>
		/// <returns>s The parent BRERuleFactory
		/// 
		/// </returns>
		virtual public IBRERuleFactory Factory
		{
			get
			{
				return factory;
			}
			
		}
		/// <summary> Returns a UID for this result set
		/// *
		/// </summary>
		/// <returns> The UID of this result
		/// 
		/// </returns>
		virtual public object Id
		{
			get
			{
				return id;
			}
			
		}
		/// <summary> Returns any parameters that were passed in to the specific instance of this
		/// Rule
		/// *
		/// </summary>
		/// <returns> An IDictionary of the parameters
		/// 
		/// </returns>
		virtual public IDictionary Parameters
		{
			get
			{
				return params_Renamed;
			}
			
		}
		/// <summary> Returns the location that the RuleResult was returned from in 
		/// the call stack
		/// *
		/// </summary>
		/// <returns> The location of the RuleResult on the stack
		/// 
		/// </returns>
		virtual public int StackLocation
		{
			get
			{
				return stackLoc;
			}
			
		}
		/// <summary> Returns the step that was called in the executeRule() within
		/// the parent BRERuleFactory.
		/// Note: Can be NULL!
		/// *
		/// </summary>
		/// <returns>The step that was called.
		/// 
		/// </returns>
		virtual public object Step
		{
			get
			{
				return step;
			}
			
		}
		private IBRERuleFactory factory = null;
		
		private object id = null;
		
		private IDictionary params_Renamed = null;
		
		private int stackLoc = 0;
		
		private object step = null;
		
		/// <summary> Protected constructor to allow for data structure population
		/// *
		/// </summary>
		/// <param name="aId">The UID of the RuleResult
		/// </param>
		/// <param name="aFactory">The RuleFactory that create the RuleResult
		/// </param>
		/// <param name="aParams">An IDictionary of the parameters
		/// </param>
		/// <param name="aStackLoc">The location on the stack of this RuleResult
		/// </param>
		/// <param name="aStep">The step within the rule
		/// 
		/// </param>
		protected internal AbstractBRERuleMetaData(object aId, IBRERuleFactory aFactory, IDictionary aParams, int aStackLoc, object aStep)
		{
			id = aId;
			factory = aFactory;
			params_Renamed = aParams;
			stackLoc = aStackLoc;
			step = aStep;
		}
		
		
		
		
		
		
		/// <summary> Display Method
		/// *
		/// </summary>
		/// <returns> String containing info
		/// 
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append("ID Type : ")
				.Append(id.GetType().FullName)
				.Append("\n")
				.Append("ID Str  : ")
				.Append(id.ToString())
				.Append("\n")
				.Append("Factory : ")
				.Append(factory.ToString())
				.Append("\n")
				.Append("Stack Loc: ")
				.Append(stackLoc);
			
			if (step != null)
				sb.Append("\n")
					.Append("Step Type: ")
					.Append(step.GetType().FullName)
					.Append("\n")
					.Append("Step Str: ")
					.Append(step.ToString());
			
			return sb.ToString();
		}
	}
}
