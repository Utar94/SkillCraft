import { _delete, get, post, put } from '.'

export async function createCondition({ description, maxLevel, name }) {
  return await post('/conditions', { description, maxLevel, name })
}

export async function deleteCondition(id) {
  return await _delete(`/conditions/${id}`)
}

export async function getCondition(id) {
  return await get(`/conditions/${id}`)
}

export async function getConditions({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/conditions?${query}`)
}

export async function updateCondition(id, { description, maxLevel, name }) {
  return await put(`/conditions/${id}`, { description, maxLevel, name })
}
