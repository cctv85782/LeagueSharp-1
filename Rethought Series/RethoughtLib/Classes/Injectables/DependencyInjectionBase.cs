//     Copyright (C) 2016 Rethought
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//     Created: 04.10.2016 1:05 PM
//     Last Edited: 04.10.2016 1:43 PM

namespace RethoughtLib.Classes.Injectables
{
    public abstract class DependencyInjectionBase<T>
    {
        #region Fields

        /// <summary>
        ///     The element
        /// </summary>
        private T element;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DependencyInjectionBase{T}" /> class.
        /// </summary>
        /// <param name="element">The element.</param>
        protected DependencyInjectionBase(T element)
        {
            this.element = element;
        }

        #endregion

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Changes the element.
        /// </summary>
        /// <param name="newElement">The new element.</param>
        public virtual void ChangeElement(T newElement)
        {
            this.element = newElement;
        }

        /// <summary>
        ///     Starts this instance.
        /// </summary>
        public abstract void Start();

        #endregion
    }
}