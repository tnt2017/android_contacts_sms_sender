using Android.App;
using Android.Widget;
using Android.OS;
using Android.Provider;
using System.Collections.Generic;
using Android.Telephony;

namespace App5
{
    [Activity(Label = "SMS-SENDER", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText editText1;
        EditText editText2;
        ListView listview1;
        List<string> itemlist;

        int count1 = 0, count2 = 0;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            editText1 = FindViewById<EditText>(Resource.Id.editText1);
            editText2 = FindViewById<EditText>(Resource.Id.editText2);

            listview1 = FindViewById<ListView>(Resource.Id.listView1);


            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += delegate {
                button1.Text = string.Format("{0} clicks", count1++);

                SmsManager.Default.SendTextMessage(editText1.Text, null, editText2.Text, null, null);
            };

            Button button2 = FindViewById<Button>(Resource.Id.button2);
            button2.Click += delegate {
                button2.Text = string.Format("{0} clicks", count2++);

                GetContacts();
            };

            listview1.ChoiceMode = (Android.Widget.ChoiceMode)ListView.ChoiceModeMultiple;

            listview1.ItemClick += (sender, e) =>
            {
                string str = listview1.GetItemAtPosition(e.Position).ToString();
                str = str.Substring(0, str.IndexOf(","));
                editText1.Text = str;
                //editText2.Text = str;
            };
        }


        void GetContacts()
        {
            itemlist = new List<string>();

            var uri = ContactsContract.Contacts.ContentUri;

            string[] projection = {
                ContactsContract.Contacts.InterfaceConsts.Id, ContactsContract.Contacts.InterfaceConsts.DisplayName,  ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber }; //ContactsContract.CommonDataKinds.Phone.Number

            var cursor = ManagedQuery(uri, projection, null, null, null);

            if (cursor.MoveToFirst())
            {
                do
                {
                    string s = "";

                    //s += "ID: {" + cursor.GetString(cursor.GetColumnIndex(projection[0])) + "}, ";
                    s += getPhoneNumber(cursor.GetString(cursor.GetColumnIndex(projection[1]))) + ", ";
                    s += cursor.GetString(cursor.GetColumnIndex(projection[1]));

                    if (cursor.GetString(cursor.GetColumnIndex(projection[2]))=="1")
                    itemlist.Add(s);

                    //checkedTextView1.Checked = true;

                } while (cursor.MoveToNext());
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, itemlist);
            listview1.Adapter = adapter;
        }

        public string getPhoneNumber(string name)
        {
            string ret = null;
            string selection = ContactsContract.Contacts.InterfaceConsts.DisplayName + " like '%" + name + "%'";
            string[] projection = new string[] { ContactsContract.CommonDataKinds.Phone.Number };
            var c = ManagedQuery(ContactsContract.CommonDataKinds.Phone.ContentUri, projection, selection, null, null);
            if (c.MoveToFirst())
            {
                ret = c.GetString(0);
            }

            return ret;
        }

    }
}

