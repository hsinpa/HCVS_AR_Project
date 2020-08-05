using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;

namespace Expect.View
{

    public class ClassInfoModal : Modal
    {

        [SerializeField]
        private Text title;

        [SerializeField]
        private Button closeBtn;

        [Header("Chart Component")]
        [SerializeField]
        private LegendView legendView;

        [SerializeField]
        private BarChart barChart;


        public void SetTitle(string titleText) {
            title.text = titleText;
        }



        private void Awake()
        {
            closeBtn.onClick.AddListener(() =>
            {
                Modals.instance.Close();
            });
        }




    }
}