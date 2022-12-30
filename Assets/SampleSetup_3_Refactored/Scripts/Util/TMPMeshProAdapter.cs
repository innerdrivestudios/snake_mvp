using TMPro;
using UnityEngine;

namespace SampleSetup_3_Refactored
{
	public class TMPMeshProAdapter : MonoBehaviour
	{
		[SerializeField] private TMP_Text label;
		[SerializeField] private string prefix;

		private void Reset()
		{
			label = GetComponent<TMP_Text>();
		}

		private void Awake()
		{
			label.text = "";
		}

		public void SetInt(int pValue)
		{
			label.text = string.IsNullOrEmpty(prefix) ? (pValue.ToString()) : (prefix.Replace("\\t", "\t") + pValue.ToString());
		}
	}
}
