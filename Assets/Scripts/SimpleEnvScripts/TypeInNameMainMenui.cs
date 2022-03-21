using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeInNameMainMenui : MonoBehaviour
{
    public string name = "";
    public List<GameObject> alphabet;
    public float interval = 0.5f;
    public GameObject ErrorDisplayCannotDelete;
    public GameObject ErrorDisplayNotChar;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && name.Length < transform.childCount) AddLetter(1, name.Length);
        else if (Input.GetKeyDown(KeyCode.B) && name.Length < transform.childCount) AddLetter(2, name.Length);
        else if (Input.GetKeyDown(KeyCode.C) && name.Length < transform.childCount) AddLetter(3, name.Length);
        else if (Input.GetKeyDown(KeyCode.D) && name.Length < transform.childCount) AddLetter(4, name.Length);
        else if (Input.GetKeyDown(KeyCode.E) && name.Length < transform.childCount) AddLetter(5, name.Length);
        else if (Input.GetKeyDown(KeyCode.F) && name.Length < transform.childCount) AddLetter(6, name.Length);
        else if (Input.GetKeyDown(KeyCode.G) && name.Length < transform.childCount) AddLetter(7, name.Length);
        else if (Input.GetKeyDown(KeyCode.H) && name.Length < transform.childCount) AddLetter(8, name.Length);
        else if (Input.GetKeyDown(KeyCode.I) && name.Length < transform.childCount) AddLetter(9, name.Length);
        else if (Input.GetKeyDown(KeyCode.J) && name.Length < transform.childCount) AddLetter(10, name.Length);
        else if (Input.GetKeyDown(KeyCode.K) && name.Length < transform.childCount) AddLetter(11, name.Length);
        else if (Input.GetKeyDown(KeyCode.L) && name.Length < transform.childCount) AddLetter(12, name.Length);
        else if (Input.GetKeyDown(KeyCode.M) && name.Length < transform.childCount) AddLetter(13, name.Length);
        else if (Input.GetKeyDown(KeyCode.N) && name.Length < transform.childCount) AddLetter(14, name.Length);
        else if (Input.GetKeyDown(KeyCode.O) && name.Length < transform.childCount) AddLetter(15, name.Length);
        else if (Input.GetKeyDown(KeyCode.P) && name.Length < transform.childCount) AddLetter(16, name.Length);
        else if (Input.GetKeyDown(KeyCode.Q) && name.Length < transform.childCount) AddLetter(17, name.Length);
        else if (Input.GetKeyDown(KeyCode.R) && name.Length < transform.childCount) AddLetter(18, name.Length);
        else if (Input.GetKeyDown(KeyCode.S) && name.Length < transform.childCount) AddLetter(19, name.Length);
        else if (Input.GetKeyDown(KeyCode.T) && name.Length < transform.childCount) AddLetter(20, name.Length);
        else if (Input.GetKeyDown(KeyCode.U) && name.Length < transform.childCount) AddLetter(21, name.Length);
        else if (Input.GetKeyDown(KeyCode.V) && name.Length < transform.childCount) AddLetter(22, name.Length);
        else if (Input.GetKeyDown(KeyCode.W) && name.Length < transform.childCount) AddLetter(23, name.Length);
        else if (Input.GetKeyDown(KeyCode.X) && name.Length < transform.childCount) AddLetter(24, name.Length);
        else if (Input.GetKeyDown(KeyCode.Y) && name.Length < transform.childCount) AddLetter(25, name.Length);
        else if (Input.GetKeyDown(KeyCode.Z) && name.Length < transform.childCount) AddLetter(26, name.Length);
        else if(Input.GetKeyDown(KeyCode.Backspace)){
            if (name.Length == 0) StartCoroutine(DisplayNotPossible(ErrorDisplayCannotDelete));
            else
            {
                Destroy(transform.GetChild(name.Length - 1).transform.GetChild(0).gameObject);
                name = name.Substring(0, name.Length - 1);
            }
        }
    }
    IEnumerator DisplayNotPossible(GameObject error)
    {
            error.SetActive(true);
            yield return new WaitForSeconds(interval);
            error.SetActive(false);
    }
    void AddLetter(int alphbetNum, int index)
    {
        GameObject letter;
        letter = Instantiate(alphabet[alphbetNum - 1], transform.GetChild(index).transform.position, Quaternion.Euler(-90, 0, 180));
        letter.transform.parent = gameObject.transform.GetChild(index).transform;
        #region Mapping Alphabet to number
        char toBeAdded = 'A';
        if (alphbetNum == 1) toBeAdded = 'A';
        else if (alphbetNum == 2) toBeAdded = 'B';
        else if (alphbetNum == 3) toBeAdded = 'C';
        else if (alphbetNum == 4) toBeAdded = 'D';
        else if (alphbetNum == 5) toBeAdded = 'E';
        else if (alphbetNum == 6) toBeAdded = 'F';
        else if (alphbetNum == 7) toBeAdded = 'G';
        else if (alphbetNum == 8) toBeAdded = 'H';
        else if (alphbetNum == 9) toBeAdded = 'I';
        else if (alphbetNum == 10) toBeAdded = 'J';
        else if (alphbetNum == 11) toBeAdded = 'K';
        else if (alphbetNum == 12) toBeAdded = 'L';
        else if (alphbetNum == 13) toBeAdded = 'M';
        else if (alphbetNum == 14) toBeAdded = 'N';
        else if (alphbetNum == 15) toBeAdded = 'O';
        else if (alphbetNum == 16) toBeAdded = 'P';
        else if (alphbetNum == 17) toBeAdded = 'Q';
        else if (alphbetNum == 18) toBeAdded = 'R';
        else if (alphbetNum == 19) toBeAdded = 'S';
        else if (alphbetNum == 20) toBeAdded = 'T';
        else if (alphbetNum == 21) toBeAdded = 'U';
        else if (alphbetNum == 22) toBeAdded = 'V';
        else if (alphbetNum == 23) toBeAdded = 'W';
        else if (alphbetNum == 24) toBeAdded = 'X';
        else if (alphbetNum == 25) toBeAdded = 'Y';
        else if (alphbetNum == 26) toBeAdded = 'Z';
        else print("Add letter to name in main menu recieved a number outside of 1-26");
        #endregion
        name = name + toBeAdded;
    }
}
