namespace RethoughtLib.Classes.FeatureV2
{
    #region Using Directives

    using global::RethoughtLib.Classes.FeatureV2.Abstract_Classes;

    #endregion

    public abstract class Child : Base
    {
        #region Public Events

        public event EventHandler NotifiyParentEvent;

        #endregion

        #region Public Methods and Operators

        public virtual void OnNotifiyParentEvent()
        {
            this.NotifiyParentEvent?.Invoke();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "Child " + this.Name;
        }

        #endregion
    }
}