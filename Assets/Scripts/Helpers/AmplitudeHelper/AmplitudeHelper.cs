using System.Collections.Generic;
using UnityEngine;

public class AmplitudeHelper : MonoBehaviour
{
	[SerializeField] private string apiKey = "";

	public static AmplitudeHelper Inctance { get; private set; }
	private Amplitude _amplitude;

	private void Awake()
	{
		Inctance ??= this;
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		Init();

		SessionFirst();
		SessionStart();
	}

	private void Init()
	{
		_amplitude = Amplitude.getInstance();
		_amplitude.setServerUrl("https://api2.amplitude.com");
		_amplitude.logging = true;
		_amplitude.trackSessionEvents(true);
		_amplitude.init(apiKey);
	}

	public void SendEvent(string eventName) => _amplitude?.logEvent(eventName);
	public void SendEvent(string eventName, Dictionary<string, object> eventProps) => _amplitude?.logEvent(eventName, eventProps);

	/// <summary>
	/// Приложение запущено первый раз
	/// </summary>
	private void SessionFirst()
	{
		var session_count = PlayerPrefs.GetInt("session_count", 0);
		if (session_count == 0)
			SendEvent("session_first");
	}

	/// <summary>
	/// При каждом запуске приложения
	/// </summary>
	private void SessionStart()
	{
		var eventProps = new Dictionary<string, object>();

		// Номер сессии пользователя
		var session_count = PlayerPrefs.GetInt("session_count", 0);
		eventProps.Add("session_count", session_count);
		PlayerPrefs.SetInt("session_count", session_count + 1);

		// Идентификатор пользователя
		var custom_id = SystemInfo.deviceUniqueIdentifier;
		eventProps.Add("custom_id", custom_id);

		SendEvent("session_start");
	}



	/// <summary>
	/// Запуск туториала
	/// </summary>
	public void TutorialStarted()
	{
		SendEvent("tutorial_started");
	}

	/// <summary>
	/// Запущен N блок туториала
	/// </summary>
	/// <param name="number">Номер блока (1,2,3...)</param>
	public void TutorialNStarted(int number)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("number", number);

		SendEvent($"tutorial_{number}_started", eventProps);
	}

	/// <summary>
	/// Закончен N блок туториала
	/// </summary>
	/// <param name="number">Номер блока (1,2,3...)</param>
	public void TutorialNCompleted(int number)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("number", number);

		SendEvent($"tutorial_{number}_completed", eventProps);
	}

	/// <summary>
	/// Туториал закончен
	/// </summary>
	public void TutorialCompleted()
	{
		SendEvent("tutorial_completed");
	}



	/// <summary>
	/// Начало уровня
	/// </summary>
	/// <param name="level_id">Номер уровня (1,2,3... N)</param>
	public void LevelStarted(int level_id)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("level_id", level_id);

		SendEvent("level_started", eventProps);
	}

	/// <summary>
	/// Уровень пройден
	/// </summary>
	/// <param name="level_id">Номер уровня (1,2,3... N)</param>
	public void LevelCompleted(int level_id)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("level_id", level_id);

		SendEvent("level_completed", eventProps);
	}

	/// <summary>
	/// Уровень проигран
	/// </summary>
	/// <param name="level_id">Номер уровня (1,2,3... N)</param>
	public void LevelFailed(int level_id)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("level_id", level_id);

		SendEvent("level_failed", eventProps);
	}

	/// <summary>
	/// Игрок нажал на кнопку рестарта уровня
	/// </summary>
	/// <param name="level_id">Номер уровня (1,2,3... N)</param>
	public void LevelRestart(int level_id)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("level_id", level_id);

		SendEvent("level_restart", eventProps);
	}

	/// <summary>
	/// Запущена N часть уровня
	/// </summary>
	/// <param name="number">Номер блока (1,2,3...)</param>
	public void LevelPartNStarted(int level_id, int number)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("level_id", level_id);
		eventProps.Add("number", number);

		SendEvent($"level_part_{number}_started", eventProps);
	}

	/// <summary>
	/// Закончена N часть уровня
	/// </summary>
	/// <param name="number">Номер блока (1,2,3...)</param>
	public void LevelPartNFinished(int level_id, int number)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("level_id", level_id);
		eventProps.Add("number", number);

		SendEvent($"level_part_{number}_finished", eventProps);
	}

	/// <summary>
	/// Игрок использовал бустер
	/// </summary>
	/// <param name="level_id">Номер уровня (1,2,3... N)</param>
	public void UsedBooster(int level_id)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("level_id", level_id);

		SendEvent("used_booster", eventProps);
	}



	/// <summary>
	/// Интерстишиал запущен
	/// </summary>
	/// <param name="placement">Место, где был запущен интер</param>
	public void InterstitialStarted(string placement)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("placement", placement);

		SendEvent("interstitial_started", eventProps);
	}

	/// <summary>
	/// Интерстишиал завершился успешно
	/// </summary>
	/// <param name="placement">Место, где был запущен интер</param>
	public void InterstitialComplete(string placement)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("placement", placement);

		SendEvent("interstitial_complete", eventProps);
	}

	/// <summary>
	/// Ревард запущен
	/// </summary>
	/// <param name="placement">Место, где был запущен реворд</param>
	public void RewardedStarted(string placement)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("placement", placement);

		SendEvent("rewarded_started", eventProps);
	}

	/// <summary>
	/// Ревард завершился успешно
	/// </summary>
	/// <param name="placement">Место, где был запущен реворд</param>
	public void RewardedComplete(string placement)
	{
		var eventProps = new Dictionary<string, object>();
		eventProps.Add("placement", placement);

		SendEvent("rewarded_complete", eventProps);
	}

	/// <summary>
	/// Баннер запущен
	/// </summary>
	public void BannerStarted()
	{
		SendEvent("banner_started");
	}

	/// <summary>
	/// Баннер завершился успешно
	/// </summary>
	public void BannerCompleted()
	{
		SendEvent("banner_completed");
	}



	/// <summary>
	/// Покупка инаппа/подписки
	/// </summary>
	public void PurchaseSuccess()
	{
		SendEvent("purchase_success");
	}

	/// <summary>
	/// Покупка инаппа
	/// </summary>
	public void PurchaseInappSuccess()
	{
		SendEvent("purchase_inapp_success");
	}

	/// <summary>
	/// Покупка подписки
	/// </summary>
	public void PurchaseSubscriptionSuccess()
	{
		SendEvent("purchase_subscription_success");
	}

	/// <summary>
	/// Показ экрана подписки
	/// </summary>
	public void SubscriptionShow()
	{
		SendEvent("subscription_show");
	}
}