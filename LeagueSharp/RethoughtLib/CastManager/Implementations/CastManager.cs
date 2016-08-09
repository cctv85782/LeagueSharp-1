namespace RethoughtLib.CastManager.Implementations
{
    #region Using Directives

    using System;
    using System.Linq;

    using global::RethoughtLib.CastManager.Abstract_Classes;

    #endregion


    /* TODO:
     * - Add Collection time-span
     * */

    /// <summary>
    ///     The CastManager.
    ///  </summary>
    public class CastManager : CastManagerBase
    {
        #region Fields

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Processes all items that are supposed to get casted.
        /// </summary>
        public override void Process()
        {
            try
            {
                for (var i = 0; i < this.Queque.Dictionary.ToList().Count; i++)
                {
                    var action = this.Queque.Dequeue();

                    action?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex: " + ex);
            }
        }

        #endregion
    }
}