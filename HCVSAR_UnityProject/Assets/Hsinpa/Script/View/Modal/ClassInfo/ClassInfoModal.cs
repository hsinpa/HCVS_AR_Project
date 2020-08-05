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

        public void SetAxisLabel(List<string> axis_label) {
            barChart.AxisConfig.HorizontalAxisConfig.ValueFormatterConfig.CustomValues.Clear();

            barChart.AxisConfig.HorizontalAxisConfig.ValueFormatterConfig.CustomValues = axis_label;
        }

        public void SetChartData(string key, Color barColor, TypeFlag.SocketDataType.ClassScoreType[] scoreArray) {

            if (scoreArray == null) return;

            BarDataSet set = new BarDataSet();
            set.Title = key;

            int scoreLen = scoreArray.Length;

            for (int i = 0; i < scoreLen; i++) {
                set.AddEntry(new BarEntry(i, scoreArray[i].main_value));
            }

            set.BarColors.Add(barColor);

            barChart.GetChartData().DataSets.Add(set);

            barChart.SetDirty();
        }

        public void ResetContent()
        {
            var chartData = barChart.GetChartData();
            if (chartData != null)
            {
                chartData.Clear();
                barChart.SetDirty();
            }
        }
    }
}