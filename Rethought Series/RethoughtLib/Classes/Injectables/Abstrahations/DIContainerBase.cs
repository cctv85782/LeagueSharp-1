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

namespace RethoughtLib.Classes.Injectables.Abstrahations
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public abstract class DiContainerBase<T> : DependencyInjectionBase<T>
    {
        #region Fields

        private readonly IList<T> elements = default(IList<T>);

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DependencyInjectionBase{T}" /> class.
        /// </summary>
        /// <param name="element">The element.</param>
        protected DiContainerBase(T element)
            : base(element)
        {
        }

        #endregion

        #endregion

        #region Public Methods and Operators

        public virtual void AddElement(T element)
        {
            this.elements.Add(element);
        }

        #endregion
    }
}