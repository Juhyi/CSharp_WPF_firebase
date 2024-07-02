using Google.Cloud.Firestore;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirestoreDocumentReference = Google.Cloud.Firestore.DocumentReference;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FirestoreDb db;

        public MainWindow()
        {
            InitializeComponent();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Add_Document_with_AutoID();
        }

        async Task Add_Document_with_AutoID()
        {
            try
            {
                CollectionReference coll = db.Collection("Add_Document_With_AutoID");
                Dictionary<string, object> data1 = new Dictionary<string, object>()
                {
                    {"FirstName", "Kim" },
                    {"LastName", "inje" },
                    {"PhoneNumber", "010-1234-5678" }
                };
                await coll.AddAsync(data1);
                MessageBox.Show("Data added successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding document: " + ex.Message);
            }
        }

        async Task Add_Document_with_CustomID()
        {
            FirestoreDocumentReference DOC = db.Collection("SearchTest").Document("TestC");
            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                {"FirestName", "Moly" },
                {"LastName","inje" },
                {"PhoneNumber", "010-1234-5789" }
            };
            await DOC.SetAsync(data1);
            MessageBox.Show("data added successfully");
        }

        async Task Add_Array()
        {
            FirestoreDocumentReference DOC = db.Collection("Add_Aray").Document("myDoc");
            Dictionary<string, object> data1 = new Dictionary<string, object>();

            ArrayList array = new ArrayList();
            array.Add(123);
            array.Add("name");
            array.Add(true);

            data1.Add("My Array", array);
            await DOC.SetAsync(data1);

            MessageBox.Show("data added successfully");
        }

        async Task Add_ListOfObjects()
        {
            FirestoreDocumentReference DOC = db.Collection("Add_ListOfObjects").Document("myDoc");
            Dictionary<string, object> MainData = new Dictionary<string, object>();
            Dictionary<string, object> List1 = new Dictionary<string, object>()
            {
                {"FirestName", "Kim" },
                {"LastName","Jinwon" },
                {"PhoneNumber", "010-1234-5678" }
            };

            MainData.Add("MyList", List1);

            await DOC.SetAsync(MainData);
            MessageBox.Show("data added successfully");
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"test-325e6-firebase-adminsdk-rnle1-777f85439c.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                db = FirestoreDb.Create("test-325e6");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing Firestore: " + ex.Message);
            }

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Add_Document_with_CustomID();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Add_Array();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            await Add_ListOfObjects();
        }
    }
}