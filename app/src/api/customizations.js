import { _delete, get, post, put } from '.'

export async function createCustomization({ description, name, type }) {
  return await post('/customizations', { description, name, type })
}

export async function deleteCustomization(id) {
  return await _delete(`/customizations/${id}`)
}

export async function getCustomization(id) {
  return await get(`/customizations/${id}`)
}

export async function getCustomizations({ deleted, search, type, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, type, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/customizations?${query}`)
}

export async function updateCustomization(id, { description, name }) {
  return await put(`/customizations/${id}`, { description, name })
}
