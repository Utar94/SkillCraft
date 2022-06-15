import { _delete, get, post, put } from '.'

export async function createAspect({ description, mandatoryAttribute1, mandatoryAttribute2, name, optionalAttribute1, optionalAttribute2, skill1, skill2 }) {
  return await post('/aspects', { description, mandatoryAttribute1, mandatoryAttribute2, name, optionalAttribute1, optionalAttribute2, skill1, skill2 })
}

export async function deleteAspect(id) {
  return await _delete(`/aspects/${id}`)
}

export async function getAspect(id) {
  return await get(`/aspects/${id}`)
}

export async function getAspects({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/aspects?${query}`)
}

export async function updateAspect(
  id,
  { description, mandatoryAttribute1, mandatoryAttribute2, name, optionalAttribute1, optionalAttribute2, skill1, skill2 }
) {
  return await put(`/aspects/${id}`, { description, mandatoryAttribute1, mandatoryAttribute2, name, optionalAttribute1, optionalAttribute2, skill1, skill2 })
}
