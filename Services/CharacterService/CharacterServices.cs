global using AutoMapper;
using AutoMapper;
using dotnetRPG.models;

namespace dotnetRPG.Services.CharacterService;

public class CharacterServices : ICharacterService
{
    private readonly IMapper _mapper;

    private static List<Character> listCharacters = new List<Character>
    {
        new Character(),
        new Character { Id = 1, Name = "Sam" }
    };

    public CharacterServices(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>()
            { Data = listCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var character = listCharacters.FirstOrDefault(c => c.Id == id);
        if (character is null)
        {
            throw new Exception("Alarm");
        }

        var serviceResponse = new ServiceResponse<GetCharacterDto>() { Data = _mapper.Map<GetCharacterDto>(character) };
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.Id = listCharacters.Max(c => c.Id) + 1;

        listCharacters.Add(character);
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>()
            { Data = listCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = listCharacters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

            if (character is null)
            {
                throw new Exception("Character not found");
            }

            _mapper.Map(updatedCharacter, character);

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }


        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var charater = listCharacters.Find(c => c.Id == id);

            if (charater is null)
            {
                throw new Exception("Character not found");
            }

            listCharacters.Remove(charater);

            serviceResponse.Data = listCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}