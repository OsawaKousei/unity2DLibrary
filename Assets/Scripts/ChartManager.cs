using System;
using UnityEngine;
using XCharts;


public class ChartManager: MonoBehaviour
{
    public GameObject testBarChart;

    private BarChart BarChart;

    Serie barSerie;

    public string[,] table = new string[5, 2];
    // Start is called before the first frame update
    void Start()
    {
        initializeBarChart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initializeBarChart()
    {
        try
        {
            BarChart = testBarChart.GetComponent<BarChart>();
            BarChart.RemoveData();
            barSerie = BarChart.AddSerie("Bar", "barChart");//グラフの形式を指定
            BarChart.AddXAxisData("null");
            barSerie.AddYData(0);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void setBarChart(string[,] input)
    {
        try
        {
            //グラフの初期化
            BarChart.ClearAxisData();
            barSerie.ClearData();

            //グラフの再描画
            for(int i = 0; i < input.GetLength(0); i++)
            {
                //横軸にデータを追加
                BarChart.AddXAxisData(input[i, 0]);
                //serieにデータの値を追加
                barSerie.AddYData(float.Parse(input[i, 1]));
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void exampleBarChart()
    {
        for(int i = 0; i < table.GetLength(0);i++) {
            table[i, 0] = (i+1) +"番目";
            table[i, 1] = ((i+1)*2).ToString();
        }
        setBarChart(table);
    }
}
