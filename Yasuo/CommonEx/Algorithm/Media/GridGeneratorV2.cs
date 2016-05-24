namespace Yasuo.CommonEx.Algorithm.Media
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using global::Yasuo.CommonEx.Algorithm.Djikstra;
    using global::Yasuo.CommonEx.Objects;

    using LeagueSharp;

    using SharpDX;

    #endregion

    class GridGeneratorV2
    {
        #region Fields

        internal SortedDictionary<int, SortedDictionary<int, Connection>> Dictionary;

        internal Grid Grid;

        internal List<Djikstra.Point> Points;

        internal List<Obj_AI_Base> Units;

        private int depth = 0;

        #endregion

        #region Constructors and Destructors

        public GridGeneratorV2()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates the grid used for pathfinding.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="deepness">The deepness.</param>
        /// <param name="units">The units.</param>
        public void Generate(Vector3 from, Vector3 to, int deepness = 10, List<Obj_AI_Base> units = null)
        {
            this.Dictionary = new SortedDictionary<int, SortedDictionary<int, Connection>>();

            this.Points = new List<Djikstra.Point>
                              { new Djikstra.Point(from) };

            this.Units = new List<Obj_AI_Base>();

            if (this.Units == null || !this.Units.Any())
            {
                return;
            }

            var walkingpath = new SortedDictionary<int, Connection>();

            var vectorarray = GlobalVariables.Player.GetPath(from, to);

            for (var i = 0; i < deepness; i++)
            {
                this.depth = i;
                
                // walking
                if (i < vectorarray.Length - 1)
                {
                    var point1 = new global::Yasuo.CommonEx.Algorithm.Djikstra.Point(vectorarray[i]);
                    var point2 = new global::Yasuo.CommonEx.Algorithm.Djikstra.Point(vectorarray[i + 1]);

                    walkingpath.Add(i,  new Connection(point1, point2) );
                }

                // dashes
                foreach (var point in this.Points)
                {
                    this.BuildDashesAroundPoint(point);
                }

                this.AddOrUpdate(walkingpath, this.depth);
            }
        }

        /// <summary>
        ///     Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.Dictionary = new SortedDictionary<int, SortedDictionary<int, Connection>>();
            this.Points = new List<Djikstra.Point>();
            this.Units = new List<Obj_AI_Base>();
            this.depth = 0;
            this.Grid = null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Backtraces from some point to the starting point.
        /// </summary>
        private void Backtrace()
        {
        }

        /// <summary>
        ///     Builds the dashes around some point.
        /// </summary>
        /// <param name="point">The point.</param>
        private void BuildDashesAroundPoint(Djikstra.Point point)
        {
            var localDic = new SortedDictionary<int, Connection>();
            var id = this.depth;

            foreach (var unit in this.Units)
            {
                var dash = new Dash(point.Position, unit);

                id = this.depth++;

                localDic.Add(id, new Connection(point, new Djikstra.Point(dash.EndPosition)));
            }

            this.AddOrUpdate(localDic, this.depth);
        }

        private void BuildWalkingPath()
        {
            
        }

        /// <summary>
        ///     Adds the dictionary or updates it.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="level">The level.</param>
        private void AddOrUpdate(SortedDictionary<int, Connection> dictionary, int level)
        {
            SortedDictionary<int, Connection> entry;

            this.Dictionary.TryGetValue(level, out entry);

            if (entry != null && entry.Any())
            {
                foreach (var newEntry in dictionary)
                {
                    entry.Add(newEntry.Key, newEntry.Value);
                }

                this.Dictionary[level] = entry;
            }
            else
            {
                this.Dictionary.Add(level, dictionary);
            }
        }

        /// <summary>
        ///     Builds the walking path around some point.
        /// </summary>
        /// <param name="point">The point.</param>
        private void BuildWalkingPathAroundPoint(Djikstra.Point point)
        {
        }

        #endregion
    }
}