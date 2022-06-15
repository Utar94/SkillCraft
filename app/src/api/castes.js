import { _delete, get, post, put } from '.'

export async function createCaste({ description, name, skill, wealthRoll }) {
  return await post('/castes', { description, name, skill, wealthRoll })
}

export async function deleteCaste(id) {
  return await _delete(`/castes/${id}`)
}

export async function getCaste(id) {
  return await get(`/castes/${id}`)
}

export async function getCastes({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/castes?${query}`)
}

export async function updateCaste(id, { description, name, skill, wealthRoll }) {
  return await put(`/castes/${id}`, { description, name, skill, wealthRoll })
}
