# Testování API - Optimaly Template

## Autentizace pro testování

### Development Auth Controller

Pro testování API endpoints vyžadujících autentizaci je k dispozici speciální controller dostupný pouze v DEBUG módu.

### Získání test tokenu

```bash
# Základní token pro test uživatele
curl -X POST https://localhost:5001/api/dev/auth/test-token \
  -H "Content-Type: application/json" \
  -d '{"email": "test@example.com"}'

# Token s rolemi
curl -X POST https://localhost:5001/api/dev/auth/test-token \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "Admin123!",
    "roles": ["Admin", "Manager"]
  }'
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "test@example.com",
  "userId": "12345-67890-...",
  "roles": ["Admin"],
  "expiresAt": "2025-02-07T10:30:00Z",
  "curlExample": "curl -H \"Authorization: Bearer eyJhbGciOiJIUzI1NiIs...\" https://localhost:5001/api/your-endpoint"
}
```

### Použití tokenu

```bash
# GET request
curl -H "Authorization: Bearer <token>" https://localhost:5001/api/templateproducts

# POST request s daty
curl -X POST https://localhost:5001/api/templateproducts \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Product",
    "description": "Test Description",
    "categoryId": 1
  }'
```

### Vyčištění test uživatelů

```bash
curl -X DELETE https://localhost:5001/api/dev/auth/test-users \
  -H "Authorization: Bearer <admin-token>"
```

## Test Controller

Pro testování exception handling a různých scénářů:

```bash
# TestNotFoundException
curl https://localhost:5001/api/test/test-not-found

# Test ValidationException
curl https://localhost:5001/api/test/test-validation

# Test BusinessException
curl https://localhost:5001/api/test/test-business

# Test obecné exception
curl https://localhost:5001/api/test/test-general

# Test úspěšného volání
curl https://localhost:5001/api/test/test-success

# Test produktů (vyžaduje autentizaci)
curl -H "Authorization: Bearer <token>" https://localhost:5001/api/test/test-products
```

## Poznámky

- Všechny test endpointy jsou dostupné pouze v DEBUG módu
- Test uživatelé mají prefix "test" v emailu pro snadnou identifikaci
- Tokeny mají platnost 7 dní
- Pro produkční prostředí použijte standardní registraci a login endpointy