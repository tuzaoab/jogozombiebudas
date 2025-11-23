using UnityEngine;

public class ScreenShakeOnClick : MonoBehaviour
{
    public float intensidade = 0.1f; 
    public float duracao = 0.1f;

    private Vector3 posOriginal;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Botão esquerdo
        {
            posOriginal = transform.position; // salva a posição atual real
            StartCoroutine(Tremer());
        }
    }

    System.Collections.IEnumerator Tremer()
    {
        float tempo = 0f;

        while (tempo < duracao)
        {
            transform.position = posOriginal + 
                new Vector3(
                    Random.Range(-intensidade, intensidade),
                    Random.Range(-intensidade, intensidade),
                    0
                );

            tempo += Time.deltaTime;
            yield return null;
        }

        transform.position = posOriginal; // volta pro lugar certo
    }
}
