using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTable_Viewer
{
    public partial class Form1 : Form
    {
        DataSet _ds = new DataSet(); // 각 반에 대한 정보를 가지고 있을 Dataset

        public Form1()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            bool bbCheckIsTable = false;

            if(_ds.Tables.Contains(cboxRegClass.Text))
            {
                bbCheckIsTable = true;
            }



            // 테이블 선언
            DataTable dt = null;

            if(!bbCheckIsTable)
            {
                // 테이블 생성
                dt = new DataTable(cboxRegClass.Text);

                DataColumn colName = new DataColumn("COL_NAME", typeof(string));
                DataColumn colSex = new DataColumn("COL_SEX", typeof(string));
                DataColumn colRef = new DataColumn("COL_REF", typeof(string));

                // DataTable에, 기존에 생성해놓은 Column들을 추가
                dt.Columns.Add(colName);
                dt.Columns.Add(colSex);
                dt.Columns.Add(colRef);
            }
            else
            {
                dt = _ds.Tables[cboxRegClass.Text];
            }
            

            // Row 자료를 등록
            DataRow row = dt.NewRow();
            row["COL_NAME"] = tboxRegName.Text;

            if (rdoRegSexMale.Checked) row["COL_SEX"] = "남자";
            else row["COL_SEX"] = "여자";

            row["COL_REF"] = tboxRegREF.Text;




            //// 지웠다가 추가 (비효율)
            //if(bbCheckIsTable)
            //{
            //    _ds.Tables.Remove(cboxRegClass.Text);
            //    _ds.Tables.Add(dt);
            //}


            if(bbCheckIsTable)
            {
                _ds.Tables[cboxRegClass.Text].Rows.Add(row);
            }
            else
            {
                // DataTable에, 기존에 생성해놓은 Row들을 추가
                dt.Rows.Add(row);
                // Dataset에 DataTable을 등록
                _ds.Tables.Add(dt);
            }


            RefreshView(); // index 갱신을 위한 이벤트 함수 호출
        }


        private void btnModify_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tboxRegName.Text))
            {
                foreach (DataRow oItem in _ds.Tables[cboxRegClass.Text].Rows)
                {
                    if (oItem["COL_NAME"].Equals(tboxRegName.Text))
                    {
                        if(rdoRegSexFemale.Checked)
                        {
                            oItem["COL_SEX"] = "여자";
                        }
                        else
                        {
                            oItem["COL_SEX"] = "남자";
                        }

                        oItem["COL_REF"] = tboxRegREF.Text;
                    }
                }

                RefreshView();
            }

        }

        private void btnViewDataDelete_Click(object sender, EventArgs e)
        {
            int iSelectRow = dgViewInfo.SelectedRows[0].Index;

            _ds.Tables[cboxViewClass.Text].Rows.RemoveAt(iSelectRow);

            RefreshView(); // index 갱신을 위한 이벤트 함수 호출
        }

        private void cboxViewClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            dgViewInfo.DataSource = _ds.Tables[cboxViewClass.Text];

            // 숫자 적는거랑 Column 크기
            // DatagridView Cell 정렬 및 Number 적용
            foreach (DataGridViewRow oRow in dgViewInfo.Rows)
            {
                oRow.HeaderCell.Value = oRow.Index.ToString();
            }
            dgViewInfo.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

    }
}
