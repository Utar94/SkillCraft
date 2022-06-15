import { _delete, get, post, put } from '.'

export async function createEducation({ description, name, skill, wealthMultiplier }) {
  return await post('/educations', { description, name, skill, wealthMultiplier })
}

export async function deleteEducation(id) {
  return await _delete(`/educations/${id}`)
}

export async function getEducation(id) {
  return await get(`/educations/${id}`)
}

export async function getEducations({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/educations?${query}`)
}

export async function updateEducation(id, { description, name, skill, wealthMultiplier }) {
  return await put(`/educations/${id}`, { description, name, skill, wealthMultiplier })
}
