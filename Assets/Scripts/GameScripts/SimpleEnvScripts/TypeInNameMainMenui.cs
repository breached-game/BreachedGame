using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeInNameMainMenui : MonoBehaviour
{
    public string playerName = "";
    public List<GameObject> alphabet;
    public float interval = 0.5f;
    public GameObject ErrorDisplayCannotDelete;
    public GameObject ErrorDisplayNotChar;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && playerName.Length < transform.childCount) AddLetter(1, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.B) && playerName.Length < transform.childCount) AddLetter(2, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.C) && playerName.Length < transform.childCount) AddLetter(3, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.D) && playerName.Length < transform.childCount) AddLetter(4, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.E) && playerName.Length < transform.childCount) AddLetter(5, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.F) && playerName.Length < transform.childCount) AddLetter(6, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.G) && playerName.Length < transform.childCount) AddLetter(7, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.H) && playerName.Length < transform.childCount) AddLetter(8, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.I) && playerName.Length < transform.childCount) AddLetter(9, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.J) && playerName.Length < transform.childCount) AddLetter(10, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.K) && playerName.Length < transform.childCount) AddLetter(11, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.L) && playerName.Length < transform.childCount) AddLetter(12, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.M) && playerName.Length < transform.childCount) AddLetter(13, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.N) && playerName.Length < transform.childCount) AddLetter(14, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.O) && playerName.Length < transform.childCount) AddLetter(15, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.P) && playerName.Length < transform.childCount) AddLetter(16, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.Q) && playerName.Length < transform.childCount) AddLetter(17, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.R) && playerName.Length < transform.childCount) AddLetter(18, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.S) && playerName.Length < transform.childCount) AddLetter(19, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.T) && playerName.Length < transform.childCount) AddLetter(20, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.U) && playerName.Length < transform.childCount) AddLetter(21, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.V) && playerName.Length < transform.childCount) AddLetter(22, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.W) && playerName.Length < transform.childCount) AddLetter(23, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.X) && playerName.Length < transform.childCount) AddLetter(24, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.Y) && playerName.Length < transform.childCount) AddLetter(25, playerName.Length);
        else if (Input.GetKeyDown(KeyCode.Z) && playerName.Length < transform.childCount) AddLetter(26, playerName.Length);
        else if(Input.GetKeyDown(KeyCode.Backspace)){
            if (playerName.Length == 0) StartCoroutine(DisplayNotPossible(ErrorDisplayCannotDelete));
            else
            {
                Destroy(transform.GetChild(playerName.Length - 1).transform.GetChild(0).gameObject);
                playerName = playerName.Substring(0, playerName.Length - 1);
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
        else print("Add letter to playerName in main menu recieved a number outside of 1-26");
        #endregion
        playerName = playerName + toBeAdded;
    }
}
