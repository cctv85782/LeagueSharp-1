namespace Rethought_Yasuo.Yasuo.Modules.Core.GridGenerator
{
    #region Using Directives

    using RethoughtLib.Algorithm.Dijkstra.ConnectionTypes;

    #endregion

    internal interface IGridGenerator
    {
        #region Public Methods and Operators

        Graph<T, TV> Generate<T, TV>() where TV : Edge<T>;

        #endregion
    }
}