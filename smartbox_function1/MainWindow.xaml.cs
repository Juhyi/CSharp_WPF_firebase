using Google.Cloud.Firestore;
using Google.Type;
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
using System.Collections.Generic;
using FirestoreDocumentReference = Google.Cloud.Firestore.DocumentReference;

namespace smartbox_function1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        FirestoreDb db;
        private string selectedBoxNum; // 선택된 택배함 번호를 저장할 변수
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"test-325e6-firebase-adminsdk-rnle1-bcd2252ffe.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                db = FirestoreDb.Create("test-325e6");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing Firestore: " + ex.Message);
            }
        }

        // 전화번호로 보관함 번호, 비밀번호 검색
        async Task Search(string phone)  
        {
            FirestoreDocumentReference docref = db.Collection("keep").Document($"{phone}");
            DocumentSnapshot snap = await docref.GetSnapshotAsync();

            if (snap.Exists)
            {
                Dictionary<string, object> city = snap.ToDictionary();
                foreach (var item in city)
                {
                    txtBox.Text += string.Format("{0}: {1}\n", item.Key, item.Value);
                }
            }

        }



        // 난수로 비번 생성 메서드
        private string GeneratePassword()
        {
            Random rand = new Random();
            int password = rand.Next(100000, 999999); // 6자리 난수 생성
            return password.ToString();
        }

        // 전화번호, 보관함 번호 입력 메서드
        void Join(string phone, string boxNum)
        {
            string password = GeneratePassword();

            FirestoreDocumentReference DOC = db.Collection("keep").Document($"{phone}");
            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                {"PhoneNumber", phone },
                {"BoxNumber", boxNum },
                {"password", password}
            };
            DOC.SetAsync(data1);

            MessageBox.Show($"보관이 완료되었습니다.택배함 번호 : {boxNum}, 비밀번호는 {password}입니다.");
            //txtPwd.Text = password;
            //txtBox.Text = boxNum;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string phone = txtphone.Text;
            string boxNum = selectedBoxNum;

            if (phone == "" || boxNum == "") //공백이 입력될 경우
            {
                MessageBox.Show("전화번호 또는 보관함번호에 공백이 있습니다.");
                return;
            }

            Join(phone, boxNum);
            //MessageBox.Show($"보관이 완료되었습니다. 택배함 번호 : {boxNum} 비밀번호 : {password}");
        }


        // 보관함 번호 선택 클릭 이벤트 헨들링
        private void btnBox_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            selectedBoxNum = clickedButton.Content.ToString().Split(' ')[1]; // 선택된 택배함 번호 저장

            string phone = txtphone.Text;
            string boxNum = selectedBoxNum;

            if (string.IsNullOrWhiteSpace(phone)) // 공백이 입력될 경우
            {
                MessageBox.Show("전화번호에 공백이 있습니다.");
                return;
            }

           
            MessageBox.Show($"선택된 택배함 번호는 {boxNum}입니다.");
        }

        // 검색버큰 클릭 이벤트 헨들링
        private async void Serchbtn_Click(object sender, RoutedEventArgs e)
        {
            string PhoneNum = TxtSearchPhone.Text;

            await Search(PhoneNum);

        }



    }
}
