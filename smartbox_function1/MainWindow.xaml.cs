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
using System;

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

        // 박스번호(document) 검색
        async Task Search(string boxNum)
        {
            FirestoreDocumentReference docref = db.Collection("keep").Document($"{boxNum}");
            DocumentSnapshot snap = await docref.GetSnapshotAsync();

            txtBox.Clear();
            if (snap.Exists)
            {
                Dictionary<string, object> city = snap.ToDictionary();
                foreach (var item in city)
                {
                    txtBox.Text += string.Format("{0}: {1}\n", item.Key, item.Value);
                }
            }

        }

        // 전화번호(filed) 검색하기
        async Task SearchByBoxNumber(string phone)
        {
            Query qref = db.Collection("keep").WhereEqualTo("PhoneNumber", phone);
            QuerySnapshot snap = await qref.GetSnapshotAsync();

            MessageBox.Show("저장 정보 출력");

            txtBox.Clear(); // 이전 검색 결과를 지웁니다.

            foreach (DocumentSnapshot docsnap in snap)
            {
                FirebaseProperty fp = docsnap.ConvertTo<FirebaseProperty>();

                if (docsnap.Exists)
                {
                    txtBox.Text += fp.PhoneNumber + "\n";
                    txtBox1.Text += fp.BoxNumber + "\n\n";
                    txtBox2.Text += fp.Password + "\n\n\n";

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
        void Join(string boxNum, string phone)
        {
            string password = GeneratePassword();

            FirestoreDocumentReference DOC = db.Collection("keep").Document($"{boxNum}");
            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                {"BoxNumber", boxNum},
                {"Password",  password},
                {"PhoneNumber", phone},
            };
            DOC.SetAsync(data1);

            MessageBox.Show($" 보관함 문이 열렸습니다. 보관함 번호 : {boxNum}, 비밀번호는 {password}입니다.");
            //txtPwd.Text = password;
            //txtBox.Text = boxNum;
        }

        // 보관함 검색 메서드 
        async Task<bool> FindBoxNum(string boxNum)
        {
            Query qref = db.Collection("keep").WhereEqualTo("BoxNumber", boxNum);
            QuerySnapshot snap = await qref.GetSnapshotAsync();

            foreach (DocumentSnapshot docsnap in snap)
            {
                if (docsnap.Exists)
                {
                    return true;
                }
            }
            return false;
        }


        // 보관 선택 클릭 이벤트 헨들러
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string phone = txtphone.Text;
            string boxNum = selectedBoxNum;

            if (phone == "" || boxNum == "") //공백이 입력될 경우
            {
                MessageBox.Show("전화번호 또는 보관함번호에 공백이 있습니다.");
                return;
            }

            bool checkBoxEmpty = await FindBoxNum(boxNum);
            if (checkBoxEmpty == true)
            {
                MessageBox.Show("이미 사용중인 보관함입니다.");
                return;
            }

            else
            {
                Join(phone, boxNum);
                MessageBox.Show("보관이 완료되었습니다.");
            }
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

        // document 검색 클릭 이벤트 헨들러 (박스번호)
        private async void Serchbtn_Click(object sender, RoutedEventArgs e)
        {
            

            string PhoneNumber = TxtSearchPhone.Text;


            await SearchByBoxNumber(PhoneNumber);

        }

        // 찾기 버튼 클릭 이벤트 헨들러
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string PhoneNumber = TxtSearchPhone.Text;

            FirestoreDocumentReference docref = db.Collection("keep").Document($"{PhoneNumber}");
            await docref.DeleteAsync();
            MessageBox.Show("보관함이 열렸습니다. 보관함에서 물건을 찾아가십시오.");

        }

        // 필드 검색 클릭 이벤트 핸들러 (전화번호)

        private async void SerchBoxbtn_Click(object sender, RoutedEventArgs e)
        {
            string PhoneNumber = TxtSearchPhone.Text;

            await Search(PhoneNumber);
        }
    }
}
