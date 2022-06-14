import { _delete, get, post, put } from '.'

export async function createNature({ attribute, description, featId, name }) {
  return await post('/natures', { attribute, description, featId, name })
}

export async function deleteNature(id) {
  return await _delete(`/natures/${id}`)
}

export async function getNature(id) {
  return await get(`/natures/${id}`)
}

export async function getNatures({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/natures?${query}`)
}

export async function updateNature(id, { attribute, description, featId, name }) {
  return await put(`/natures/${id}`, { attribute, description, featId, name })
}
