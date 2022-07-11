import { _delete, get /*, post, put*/ } from '.'

// export async function createCharacter({ description, name, otherRequirements, otherTalentsText, talents, tier, uniqueTalentId }) {
//   return await post('/characters', { description, name, otherRequirements, otherTalentsText, talents, tier, uniqueTalentId })
// }

export async function deleteCharacter(id) {
  return await _delete(`/characters/${id}`)
}

// export async function getCharacter(id) {
//   return await get(`/characters/${id}`)
// }

export async function getCharacters({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/characters?${query}`)
}

// export async function updateCharacter(id, { description, name, otherRequirements, otherTalentsText, talents, uniqueTalentId }) {
//   return await put(`/characters/${id}`, { description, name, otherRequirements, otherTalentsText, talents, uniqueTalentId })
// }
