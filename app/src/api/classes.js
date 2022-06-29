import { _delete, get, post, put } from '.'

export async function createClass({ description, name, otherRequirements, otherTalentsText, talents, tier, uniqueTalentId }) {
  return await post('/classes', { description, name, otherRequirements, otherTalentsText, talents, tier, uniqueTalentId })
}

export async function deleteClass(id) {
  return await _delete(`/classes/${id}`)
}

export async function getClass(id) {
  return await get(`/classes/${id}`)
}

export async function getClasses({ deleted, search, tiers, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, tiers, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/classes?${query}`)
}

export async function updateClass(id, { description, name, otherRequirements, otherTalentsText, talents, uniqueTalentId }) {
  return await put(`/classes/${id}`, { description, name, otherRequirements, otherTalentsText, talents, uniqueTalentId })
}
