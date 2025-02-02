using UnityEngine;
using System.Collections;
using static WorldData;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public static bool isInventoryOpen = false;

    public GameObject inventory;

	private CreatorsManager cManager;
	private MainController mController;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else Destroy(gameObject);
	}

	private void Start()
	{
		cManager = CreatorsManager.instance;
		mController = MainController.instance;
		inventory.SetActive(false);
		isInventoryOpen = inventory.activeSelf;
	}

	private void Update()
    {
		if (INPUT.MainController.Inventory.triggered)
		{
            inventory.SetActive(!inventory.activeSelf);
			isInventoryOpen = inventory.activeSelf;
			cManager.OnInventoryStatusChanged(inventory.activeSelf);
		}
    }

	public void AnimateText(Animator animator)
	{
		StopAllCoroutines();
		StartCoroutine(_AnimateText(animator));
	}

	private IEnumerator _AnimateText(Animator animator)
	{
		animator.SetBool("Selected", true);
		yield return new WaitForSeconds(1f);
		animator.SetBool("Selected", false);
	}

	public void EnterBuilding()
	{
		BuildingInScene bInS = mController.interaction.GetComponentInParent<BuildingInScene>();

		if (bInS == null)
		{
			bInS = mController.interaction.GetComponentInParent<BuildingInScene>();
			if (bInS == null)
				bInS = mController.interaction.transform.parent.gameObject.AddComponent<BuildingInScene>();
		}

		TilemapData tData;
		if (bInS.tilemapData == null)
			tData = new TilemapData();
		else tData = bInS.tilemapData;

		ObjectsData oData;
		if (bInS.objectsData == null)
			oData = new ObjectsData();
		else oData = bInS.objectsData;

		int index = bInS.data.index;
		float[] position = JASUtils.Utils.Vector3ToFloatArray(bInS.transform.position);
		string name = $"{index}_{bInS.transform.position}";
		WorldData.BuildingData bData = new WorldData.BuildingData(tData, oData, index, position, name);
		GameManager.curBuilding = bData;
		SceneLoader.instance.EnterBuilding(bData);
	}
	public void ExitBuilding() => SceneLoader.instance.ExitBuilding();
}
