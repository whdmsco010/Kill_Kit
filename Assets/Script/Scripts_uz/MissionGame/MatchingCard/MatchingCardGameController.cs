using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingCardGameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    /*
    void Awake(){
        puzzles = Resources.LoadAll<Sprite> ("경로명");
    }
    */

    void Start()
    {
        GetButtons();
        AddListeners();    
    }

    void GetButtons(){
        GameObject[] objects = GameObject.FindGameObjectsWithTag ("PuzzleButton");

        for ( int i= 0; i < objects.Length; i++){
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddListeners(){
        foreach (Button btn in btns) {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }

    public void PickPuzzle(){
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("No."+ name + " Clicked ");
    }

}
