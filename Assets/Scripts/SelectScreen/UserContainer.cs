using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.SelectScreen
{
    public class UserContainer : MonoBehaviour
    {
        private string _name;
        //private int box_width_diff = 30; //1文字毎の変化基準幅
        //private int standard_length = 7; //初期の幅は7文字仕様のため，7文字より何文字大きいか小さいかで幅を伸縮させる
        public Text displayed; //表示される文字列

        public void Start()
        {
            //int len = 0; //ユーザ名の文字数
            //float box_width = gameObject.GetComponent<RectTransform>().sizeDelta.x;
            //float box_height = gameObject.GetComponent<RectTransform>().sizeDelta.y;
            //float box_x = gameObject.GetComponent<RectTransform>().localPosition.x;
            //float box_y = gameObject.GetComponent<RectTransform>().localPosition.y;
            //float box_z = gameObject.GetComponent<RectTransform>().localPosition.z;

            _name = PlayerPrefs.GetString("name", "default");
            //if (Name != null) len = Name.Length;

            //RectTransform rt = gameObject.GetComponent<RectTransform>();
            //rt.sizeDelta = new Vector2(box_width + box_width_diff * (-standard_length + len), box_height);
            //rt.localPosition = new Vector3(box_x - box_width_diff * (-standard_length + len) / 2.0f, box_y, box_z);
            
            displayed.text = string.Concat("Welcome " + _name);
        }
    }
}