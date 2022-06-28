import { _delete, get, post, put } from '.'

export async function createPower({ concentration, descriptions, duration, focus, incantation, ingredients, name, range, ritual, somatic, tier, verbal }) {
  return await post('/powers', { concentration, descriptions, duration, focus, incantation, ingredients, name, range, ritual, somatic, tier, verbal })
}

export async function deletePower(id) {
  return await _delete(`/powers/${id}`)
}

export async function getPower(id) {
  return await get(`/powers/${id}`)
}

export async function getPowers({ deleted, search, tiers, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, tiers, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/powers?${query}`)
}

export async function updatePower(id, { concentration, descriptions, duration, focus, incantation, ingredients, name, range, ritual, somatic, verbal }) {
  return await put(`/powers/${id}`, { concentration, descriptions, duration, focus, incantation, ingredients, name, range, ritual, somatic, verbal })
}
