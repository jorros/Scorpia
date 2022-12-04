using Scorpian;

namespace Scorpia.Game;

public class ScorpiaSettings
{
    private readonly UserDataManager _userDataManager;

    public ScorpiaSettings(UserDataManager userDataManager)
    {
        _userDataManager = userDataManager;
    }

    public string PlayerName
    {
        get => _userDataManager.Get("player_name", string.Empty);
        set => _userDataManager.Set("player_name", value);
    }
    
    public string Identifier
    {
        get
        {
            var id = _userDataManager.Get("uin", string.Empty);
            if (Guid.TryParse(id, out _))
            {
                return id;
            }
            
            id = Guid.NewGuid().ToString();
            _userDataManager.Set("uin", id);
            return id;
        }
    }
}