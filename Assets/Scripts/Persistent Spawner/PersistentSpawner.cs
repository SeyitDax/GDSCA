using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject spawn;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MethodToExecute();
    }

    private void MethodToExecute()
    {
        Instantiate(spawn, FindObjectOfType<Canvas>().transform);
    }

    private void ErrorHandler()
    {
        /*
        YEDEK KODLARINIZI KAYDED�N
        Bu yedek kodlar�, g�venli ancak eri�ebilece�iniz bir yerde saklay�n.
        
        1. 5949 5466         6. 0407 9334
        2. 2188 6900         7. 5035 6465
        3. 2765 9439         8. 2104 9092
        4. 8629 9644         9. 9297 5380
        5. 7288 0697        10. 5495 4028
        
        (ahmetsadhacker@gmail.com)
        
        * Her bir yedek kodu yaln�zca bir kez kullanabilirsiniz.
        * Daha fazla koda m� ihtiyac�n�z var? https://g.co/2sv adresini ziyaret edin
        *Bu kodlar�n olu�turuldu�u tarih: 22 �ub 2024
        
        
        YEDEK KODLARINIZI KAYDED�N
        Bu yedek kodlar�, g�venli ancak eri�ebilece�iniz bir yerde saklay�n.
        
        1. 8743 6578		 6. 2118 8211
        2. 7934 1143		 7. 2407 7220
        3. 6551 7288		 8. 7219 5340
        4. 2043 9888		 9. 3820 2145
        5. 5365 2940		10. 7334 7966
        
        (ahmetsaddemir@gmail.com)
        
        * Her bir yedek kodu yaln�zca bir kez kullanabilirsiniz.
        * Daha fazla koda m� ihtiyac�n�z var? https://g.co/2sv adresini ziyaret edin
        * Bu kodlar�n olu�turuldu�u tarih: 13 �ub 2024
        
        */
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
