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

namespace Login_join
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
        // 로드 이벤트
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"test-325e6-firebase-adminsdk-rnle1-777f85439c.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("test-325e6");
        }

        // 회원가입 버튼 이벤트
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string id = textBox1.Text;
            string pass = textBox2.Text;

            if (id == "" || pass == "") //공백이 입력될 경우
            {
                MessageBox.Show("아이디 또는 비밀번호에 공백이 있습니다.");
                return;
            }
            JoinManagement(id, pass);
        }

        // 로그인 버튼 이벤트
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string id = textBox1.Text;
            string pass = textBox2.Text;

            if (id == "" || pass == "") //공백이 입력될 경우
            {
                MessageBox.Show("아이디 또는 비밀번호에 공백이 있습니다.");
                return;
            }

            LoginManagement(id, pass);
        }

        //회원가입 관리
        private async void JoinManagement(string id, string pass)
        {
            bool idCheck = await FindId(id);
            if (idCheck) { } //id가 이미 있으므로 회원가입 X
            else if (!idCheck) //id가 없으므로 회원가입 O
            {
                Join(id, pass);
                MessageBox.Show("회원가입이 완료되었습니다.");
            }
        }

        //로그인 관리
        private async void LoginManagement(string id, string pass)
        {
            bool idCheck = await FindId(id, pass);
            if (idCheck) //id, pass 일치
            {
                MessageBox.Show("로그인 되었습니다.");
            }
            else if (!idCheck) //id, pass 일치하지 않음
            {
                MessageBox.Show("로그인에 실패하였습니다.");
            }
        }

        //아이디 찾는 메서드
        async Task<bool> FindId(string id)
        {
            Query qref = db.Collection("Join").WhereEqualTo("Id", id);
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

        //아이디, 비번 찾는 메서드
        async Task<bool> FindId(string id, string pass)
        {
            Query qref = db.Collection("Join").WhereEqualTo("Id", id).WhereEqualTo("Pass", pass);
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

        //가입하는 메서드
        void Join(string id, string pass)
        {
            FirestoreDocumentReference DOC = db.Collection("Join").Document();
            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                {"Id", id },
                {"Pass", pass },
            };
            DOC.SetAsync(data1);
        }

    }
}