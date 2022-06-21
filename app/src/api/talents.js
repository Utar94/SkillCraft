import { _delete, get, post, put } from '.'

export async function createTalent({ description, multipleAcquisition, name, requiredTalentId, tier }) {
  return await post('/talents', { description, multipleAcquisition, name, requiredTalentId, tier })
}

export async function deleteTalent(id) {
  return await _delete(`/talents/${id}`)
}

export async function getTalent(id) {
  return await get(`/talents/${id}`)
}

export async function getTalents({ deleted, multipleAcquisition, search, tiers, sort, desc, index, count }) {
  const query = Object.entries({ deleted, multipleAcquisition, search, tiers, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/talents?${query}`)
}

export async function updateTalent(id, { description, multipleAcquisition, name, requiredTalentId }) {
  return await put(`/talents/${id}`, { description, multipleAcquisition, name, requiredTalentId })
}
