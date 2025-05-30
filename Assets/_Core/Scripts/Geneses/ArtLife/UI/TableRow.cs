using TMPro;
using UnityEngine;

namespace Geneses.ArtLife.UI
{
    public class TableRow : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI Cell1Text;
        [SerializeField] TextMeshProUGUI Cell2Text;

        public void SetContent(string cell1Text, string cell2Text)
        {
            Cell1Text.text = cell1Text;
            Cell2Text.text = cell2Text; 
        }        
    }
}
