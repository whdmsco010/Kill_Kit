using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrderScript : MonoBehaviour
{
    private int Timer = 0;
    public GameObject IMG_tutorial; // Ʃ�丮�� �̹���
    public GameObject Num_A;   //1��
    public GameObject Num_B;   //2��
    public GameObject Num_C;   //3��
    public GameObject Num_GO;   //���� �̹���
    public GameObject Num_GOO;

    void Start()
    {
        //���۽� ī��Ʈ �ٿ� �ʱ�ȭ, ���� ���� false ����
        Timer = 0;
        // Ʃ�丮��, ������ (ī��Ʈ�ٿ� �̹���) �Ⱥ��̱�
        IMG_tutorial.SetActive(false);
        Num_A.SetActive(false);
        Num_B.SetActive(false);
        Num_C.SetActive(false);
        Num_GO.SetActive(false);
        Num_GOO.SetActive(false);
    }

    void Update()
    {
        //���� ���۽� ����
        if (Timer == 0)
        {
            Time.timeScale = 0.0f;
        }
        //Timer �� 90���� �۰ų� ������� Timer �������
        if (Timer <= 1800)
        {
            Timer++;


            // Timer�� 60���� ������� Ʃ�丮�� �ѱ�
            if (Timer < 600)
            {
                IMG_tutorial.SetActive(true);
            }
            // Timer�� 60���� Ŭ��� Ʃ�丮�� ���� 3�ѱ�
            if (Timer > 600)
            {
                IMG_tutorial.SetActive(false);
                Num_C.SetActive(true);
            }
            // Timer�� 90���� ������� 3���� 2�ѱ�
            if (Timer > 900)
            {
                Num_C.SetActive(false);
                Num_B.SetActive(true);
            }
            // Timer �� 120���� Ŭ��� 2���� 1�ѱ�
            if (Timer > 1200)
            {
                Num_B.SetActive(false);
                Num_A.SetActive(true);
            }
            if (Timer > 1500)
            {
                Num_A.SetActive(false);
                Num_GO.SetActive(true);
            }
            // Timer �� 150���� ũ�ų� ���� ��� 1���� ���� �ѱ� LoadingEnd () �ڷ�ƾȣ��
            if (Timer >= 1800)
            {
                Num_GO.SetActive(false);
                Num_GOO.SetActive(true);
                StartCoroutine(this.LoadingEnd());
                Time.timeScale = 1.0f; //���ӽ���
            }
        }
    }

    IEnumerator LoadingEnd()
    {
        yield return new WaitForSeconds(1.0f);
        Num_GOO.SetActive(true);
    }

}

