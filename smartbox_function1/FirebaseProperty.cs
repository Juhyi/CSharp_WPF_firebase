using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartbox_function1
{
    [FirestoreData]
    internal class FirebaseProperty
    {
        [FirestoreProperty]
        //필드 이름
        public string PhoneNumber { get; set; }
        [FirestoreProperty]
        public string BoxNumber { get; set; }
        [FirestoreProperty]
        public string Password { get; set; }

    }
}
