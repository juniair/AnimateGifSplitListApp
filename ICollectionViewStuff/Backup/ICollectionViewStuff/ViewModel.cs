using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace ICollectionViewStuff
{
    /// <summary>
    /// ViewModel with dummy data just for demo purposes
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        #region Properties
        private ObservableCollection<Person> myData;

        /// <summary>
        /// Gets the list of persons to show in the UI
        /// </summary>
        public ObservableCollection<Person> MyData
        {
            get { return myData; }
        }

        #region Properties used for Filtering
        string searchText = String.Empty;
        /// <summary>
        /// Gets and sets the text used for searching.
        /// Please note that when this property is set a search will be executed
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                myDataView.Filter = FilterData;
                OnPropertyChanged("SearchText");
            }
        }
        #endregion


        #region Properties used for sorting
        private bool sortById = false;
        /// <summary>
        /// Gets or sets a flag indicating if the data should be sorted by ID
        /// Please note that by setting this value the list of data will get sorted
        /// </summary>
        public bool SortById
        {
            get { return sortById; }
            set
            {
                sortById = value;
                SortData();
                OnPropertyChanged("SortById");
            }
        }

        private bool sortByName = false;
        /// <summary>
        /// Gets or sets a flag indicating if the data should be sorted by Name
        /// Please note that by setting this value the list of data will get sorted
        /// </summary>
        public bool SortByName
        {
            get { return sortByName; }
            set
            {
                sortByName = value;
                SortData();
                OnPropertyChanged("SortByName");
            }
        }

        private bool sortAscending = false;
        /// <summary>
        /// Gets or sets a flag indicating the order in which the data should be sorted
        /// Please note that by setting this value the list of data will get sorted
        /// </summary>
        public bool SortAscending
        {
            get { return sortAscending; }
            set
            {
                sortAscending = value;
                SortData();
                OnPropertyChanged("SortAscending");
            }
        }
        #endregion

        #region Grouping properties
        bool groupByLocation = false;
        /// <summary>
        /// Gets or sets a flag indicating if the data should be grouped
        /// Please note that by setting this value the list of data will get grouped
        /// </summary>
        public bool GroupByLocation
        {
            get { return groupByLocation; }
            set
            {
                groupByLocation = value;
                GroupData();
                OnPropertyChanged("GroupByLocation");
            }
        }
        #endregion

        #region Selected Item
        Person currentSelectedPerson;
        /// <summary>
        /// Gets the currently selected Person from the list
        /// </summary>
        public Person CurrentSelectedPerson
        {
            get { return currentSelectedPerson; }
            private set
            {
                currentSelectedPerson = value;
                OnPropertyChanged("CurrentSelectedPerson");
            }
        }
        #endregion

        #endregion

        ICollectionView myDataView;

        SampleModel model;

        /// <summary>
        /// Constructor
        /// This constructor accepts a Model as parameter for Dependency injection so that you can pass mock models to the ViewModel while unit testing.
        /// Please note that I did not create an interface for the model because this is just a Demo and I am not mocking the Model.
        /// </summary>
        /// <param name="model">The model instance</param>
        public ViewModel(SampleModel model)
        {
            this.model = model;

            myData = new ObservableCollection<Person>(model.GenerateSampleData());

            //get the Collection view for the list
            myDataView = CollectionViewSource.GetDefaultView(myData);

            //when the current selected changes store it in the CurrentSelectedPerson
            myDataView.CurrentChanged += delegate
            {
                //stores the current selected person
                CurrentSelectedPerson = (Person)myDataView.CurrentItem;
            };
        }

        #region Filtering
        //Filter the list of data
        private bool FilterData(object item)
        {
            var value = (Person)item;
            if (value == null || value.Name == null)
                return false;

            return value.Name.Contains(SearchText);
        }
        #endregion

        #region Sorting

        //Sort the Data
        // for a custom way how to sort the data (and also a much more performant way see: http://ligao101.wordpress.com/2007/07/31/a-much-faster-sorting-for-listview-in-wpf/
        // I implemented this just to show how you can set up some easy sorting for an ICollectionView only.
        private void SortData()
        {
            //get the sort direction
            ListSortDirection direction = SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            using (myDataView.DeferRefresh()) // we use the DeferRefresh so that we refresh only once
            {
                myDataView.SortDescriptions.Clear();

                if (SortById)
                    myDataView.SortDescriptions.Add(new SortDescription("ID", direction));
                if (SortByName)
                    myDataView.SortDescriptions.Add(new SortDescription("Name", direction));
            }
        }

        #endregion

        #region Grouping
        //groups the data by using a simple PropertyGroupDescription
        private void GroupData()
        {
            using (myDataView.DeferRefresh())
            {
                myDataView.GroupDescriptions.Clear();
                if (GroupByLocation)
                    myDataView.GroupDescriptions.Add(new PropertyGroupDescription("Location"));
            }
        }
        #endregion

        #region Selection Navigation Code
        //The code for the Navigation may look like an overhead in the ViewModel but keep in mind that in a real life application you will be doing validation and what not inside here!

        public bool CanSelectFirstItem()
        {
            return myDataView.CurrentPosition > 0;
        }
        public void SelectFirstItem()
        {
            myDataView.MoveCurrentToFirst();
        }

        public bool CanSelectLastItem()
        {
            //Please note that this is done in such a way and not in myData.Count because if there is filtering being applied we need to make sure that the count of items is correct!
            return myDataView.CurrentPosition < myDataView.Cast<Person>().Count() - 1;
        }
        public void SelectLastItem()
        {
            myDataView.MoveCurrentToLast();
        }

        public bool CanSelectNextItem()
        {
            return CanSelectLastItem();
        }
        public void SelectNextItem()
        {
            myDataView.MoveCurrentToNext();
        }

        public bool CanSelectPreviousItem()
        {
            return CanSelectFirstItem();
        }
        public void SelectPreviousItem()
        {
            myDataView.MoveCurrentToPrevious();
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
