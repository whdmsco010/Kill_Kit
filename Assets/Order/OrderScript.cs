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
    public GameObject Num_D;   //���� �̹���
    public GameObject Num_E;
    public GameObject Num_F;
    public GameObject Num_G;
    public GameObject Num_H;
    public GameObject Num_I;
    

    void Start()
    {
        //���۽� ī��Ʈ �ٿ� �ʱ�ȭ, ���� ���� false ����
        Timer = 0;
        // Ʃ�丮��, ������ (ī��Ʈ�ٿ� �̹���) �Ⱥ��̱�
        IMG_tutorial.SetActive(false);
        Num_A.SetActive(false);
        Num_B.SetActive(false);
        Num_C.SetActive(false);
        Num_D.SetActive(false);
        Num_E.SetActive(false);
        Num_F.SetActive(false);
        Num_G.SetActive(false);
        Num_H.SetActive(false);
        Num_I.SetActive(false);
        
    }

    void Update()
    {
        //���� ���۽� ����
        if (Timer == 0)
        {
            Time.timeScale = 0.0f;
        }
        //Timer �� 90���� �۰ų� ������� Timer �������
        if (Timer <= 1900)
        {
            Timer++;


            // Timer�� 60���� ������� Ʃ�丮�� �ѱ�
            if (Timer < 100)
            {
                IMG_tutorial.SetActive(true);
            }
            // Timer�� 60���� Ŭ��� Ʃ�丮�� ���� 3�ѱ�
            if (Timer > 100)
            {
                IMG_tutorial.SetActive(false);
                Num_A.SetActive(true);
            }
            // Timer�� 90���� ������� 3���� 2�ѱ�
            if (Timer > 300)
            {
                Num_A.SetActive(false);
                Num_B.SetActive(true);
            }
            // Timer �� 120���� Ŭ��� 2���� 1�ѱ�
            if (Timer > 500)
            {
                Num_B.SetActive(false);
                Num_C.SetActive(true);
            }
            if (Timer > 700)
            {
                Num_C.SetActive(false);
                Num_D.SetActive(true);
            }
            if (Timer > 900)
            {
                Num_D.SetActive(false);
                Num_E.SetActive(true);
            }
            if (Timer > 1100)
            {
                Num_E.SetActive(false);
                Num_F.SetActive(true);
            }
            if (Timer > 1300)
            {
                Num_F.SetActive(false);
                Num_G.SetActive(true);
            }
            if (Timer > 1500)
            {
                Num_G.SetActive(false);
                Num_H.SetActive(true);
            }
            
            // Timer �� 150���� ũ�ų� ���� ��� 1���� ���� �ѱ� LoadingEnd () �ڷ�ƾȣ��
            if (Timer >= 1600)
            {
                Num_H.SetActive(false);
                Num_I.SetActive(true);
                StartCoroutine(this.LoadingEnd());
                Time.timeScale = 1.0f; //���ӽ���
            }
        }
    }

    IEnumerator LoadingEnd()
    {
        yield return new WaitForSeconds(1.0f);
        Num_I.SetActive(true);
    }

}