using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ICollectionViewStuff
{
    public static class Commands
    {
        #region SelectLast

        /// <summary>
        /// The SelectLast command ....
        /// </summary>
        public static RoutedUICommand SelectLast
            = new RoutedUICommand("Select Last Person", "SelectLast", typeof(Commands));

        #endregion

        #region SelectFirst

        /// <summary>
        /// The SelectFirst command ....
        /// </summary>
        public static RoutedUICommand SelectFirst
            = new RoutedUICommand("Select First Person", "SelectFirst", typeof(Commands));

        #endregion

        #region SelectNext

        /// <summary>
        /// The SelectNext command ....
        /// </summary>
        public static RoutedUICommand SelectNext
            = new RoutedUICommand("Select next person", "SelectNext", typeof(Commands));

        #endregion

        #region SelectPrevious

        /// <summary>
        /// The SelectPrevious command ....
        /// </summary>
        public static RoutedUICommand SelectPrevious
            = new RoutedUICommand("Select previous person", "SelectPrevious", typeof(Commands));

        #endregion

    }
}
