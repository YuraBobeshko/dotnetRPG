global using AutoMapper;
using AutoMapper;
using dotnetRPG.Data;
using dotnetRPG.models;

namespace dotnetRPG.Services.CharacterService;

public class CharacterServices : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    private static List<Character> listCharacters = new List<Character>
    {
        new Character(),
        new Character { Id = 1, Name = "Sam" }
    };

    public CharacterServices(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var characters = await _context.Characters.ToListAsync();
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>()
            { Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters.FindAsync(id);
        
            if (character is null)
            {
                throw new Exception("Alarm");
            }

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }


        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var character = _mapper.Map<Character>(newCharacter);
        
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();

            var characters = await _context.Characters.ToListAsync();

            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters.FindAsync(updatedCharacter.Id);

            if (character is null)
            {
                throw new Exception("Character not found");
            }
            

            _mapper.Map(updatedCharacter, character);
            _context.SaveChanges();
            
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
            var charater = await _context.Characters.FindAsync(id);

            if (charater is null)
            {
                throw new Exception("Character not found");
            }

            _context.Characters.Remove(charater);
            _context.SaveChanges();
            
            serviceResponse.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}