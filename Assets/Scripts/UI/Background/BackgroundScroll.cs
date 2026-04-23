using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField, Range(-1, 1)]
	private float _scrollSpeedX = 1.0f;
	[SerializeField, Range(-1, 1)]
	private float _scrollSpeedY = 1.0f;

	private Image _image;
	private Material _material;

	void Start()
	{
		_image = gameObject.GetComponent<Image>();

		// 他に影響を与えないためにマテリアルを複製
		_material = new Material(_image.material);
		_image.material = _material;
	}

	void Update()
	{
		if (_image == null || _material == null)
			return;

		// 現在のオフセットを取得
		Vector2 offset = _image.material.mainTextureOffset;

		// X軸のスクロール
		if (_scrollSpeedX != 0.0f)
		{
			offset.x += _scrollSpeedX * Time.deltaTime;
		}

		// Y軸のスクロール
		if (_scrollSpeedY != 0.0f)
		{
			offset.y += _scrollSpeedY * Time.deltaTime;
		}

		// 更新したオフセットをマテリアルに適用
		_image.material.mainTextureOffset = offset;
	}

	private void OnDestroy()
	{
		// マテリアルの消去
		Destroy(_material);
	}
}