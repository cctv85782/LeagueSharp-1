using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.TargetValidator
{
    using global::RethoughtLib.Classes.Injectables;
    using global::RethoughtLib.Classes.Injectables.Abstrahations;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class TargetValidator : DiContainerBase<ICheckable>
    {
        /// <summary>
        /// The invalid states
        /// </summary>
        private readonly List<ICheckable> invalidStates = new List<ICheckable>();

        /// <summary>
        /// The object
        /// </summary>
        private Obj_AI_Base target;

        private bool valid = true;

        /// <summary>
        /// Checks the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        public bool Check(Obj_AI_Base @object)
        {
            this.target = @object;

            this.Start();

            return this.valid;
        }

        /// <summary>
        /// Adds the state.
        /// </summary>
        /// <param name="state">The state.</param>
        public void AddCheck(ICheckable state)
        {
            this.invalidStates.Add(state);
        }

        /// <summary>
        /// Removes the state.
        /// </summary>
        /// <param name="state">The state.</param>
        public void RemoveCheck(ICheckable state)
        {
            this.invalidStates.Remove(state);   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionBase{T}"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public TargetValidator(ICheckable element)
            : base(element)
        {
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start()
        {
            foreach (var state in this.invalidStates)
            {
                if (this.valid == false)
                {
                    break;
                }

                this.valid = state.Check(this.target);
            }

            Console.WriteLine($"Target {this.target.Name} is an valid target: {this.valid}");
        }
    }

    public interface ICheckable
    {
        bool Check(Obj_AI_Base target);
    }

    internal class ExampleState : ICheckable
    {
        public bool Check(Obj_AI_Base target)
        {
            return target.IsValidTarget();
        }
    }
}
