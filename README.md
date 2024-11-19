# Proyecto Base .NET 8 usando buenas prácticas.
### 1- Métodos async en los controlladores (_db.Remove, _db.Update no son async)
### 2- ActionResult 
### 3- Usar [ProduceResponse] tipos de respuesta
```bash
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
```
### 4- Hacer los DTO separado del modelo hacer una para VillaCreateDto(sin id) y otro para VillaUpdateDto o mantener los Dto separados uno para create otro update.
### 5- Validaciones en los modelos y Dto [Requiered] usando if (!ModelState.IsValid) en el controlador.
```bash
 public class VillaCreateDTO
 {
     [Required]
     [MaxLength(30)]
     public string Name { get; set; }
     public string Detail { get; set; }
     [Required]
     public double Price { get; set; }
```

```bash
 public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDto)
 {
     try
     {
         if (!ModelState.IsValid)
         {
             return BadRequest(ModelState);
         }
...
```
### 6- Usar HttpPatch para modificar un solo atr en entidades grandes.
### 7- Incluir logger
```bash
 [HttpGet("id:int", Name = "GetVilla")]
 [ProducesResponseType(200)]
 [ProducesResponseType(400)]
 [ProducesResponseType(404)]
 public async Task<ActionResult<APIResponse>> GetVilla(int id)
 {
     try
     {
         if (id == 0)
         {
             _logger.LogError("Error with village id");
             _response.statusCode=HttpStatusCode.BadRequest;
             _response.IsExitoso = false;
             return BadRequest(_response);
         }
...
```
### 8- En los POST se debe retornar la URL del nuevo registro.
```bash
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
```
### 9- AsNoTracking para sacar el tracked en httpPatch.
### 10- Usar Automapper para modelos grandes.
```bash
 public class MappingConfig:Profile
 {

     public MappingConfig()
     {
         CreateMap<Villa, VillaDto>();
         CreateMap<VillaDto, Villa>();

         CreateMap<Villa, VillaCreateDTO>().ReverseMap();
         CreateMap<Villa,VillaUpdateDto>().ReverseMap();
...
```

```bash
   }
   [ProducesResponseType(204)]
   [ProducesResponseType(400)]
   [HttpPut("{id:int}")]
   public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
   {
       if (updateDto == null || id != updateDto.Id)
       {
           _response.IsExitoso=false;
           _response.statusCode = HttpStatusCode.BadRequest;
           return BadRequest(_response);
       }

      ** Villa model =_mapper.Map<Villa>(updateDto); **

   
       await _villaRepo.Update(model);
       _response.statusCode=HttpStatusCode.NoContent;

       return Ok(_response);


   }
```

### 11- En Prod no interecciona desde el controlador con la bd directamente se usan los repositorios ( Usar Patrón Repositorio).
```bash

    public class Repositorio<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repositorio(ApplicationDbContext db)
        {
            _db = db;  
            this.dbSet=_db.Set<T>();
        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro!=null) {
            query=query.Where(filtro);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }
...
```
### 12- Api Response (Para que todos los endpoints retornen una respuesta standard).
```bash
  [HttpDelete("{id:int}")]
  [ProducesResponseType(204)]
  [ProducesResponseType(400)]
  [ProducesResponseType(404)]
  public async Task<IActionResult> DeleteVilla(int id)
  {
      try
      {
          if (id == 0)
          {
              _response.IsExitoso= false;
              _response.statusCode = HttpStatusCode.BadRequest;
              return BadRequest(_response);
          }
          var villa = await _villaRepo.Get(v => v.Id == id);
          if (villa == null)
          {
              _response.IsExitoso=false;
              _response.statusCode = HttpStatusCode.NotFound;
              return NotFound(_response);
          }
          await _villaRepo.Remove(villa);
          _response.statusCode=HttpStatusCode.NoContent;
          return Ok(_response);
      }
      catch (Exception ex)
      {

          _response.IsExitoso = false;
          _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return BadRequest(_response);
     
  }
...
```
