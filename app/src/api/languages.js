import { _delete, get, post, put } from '.'

export async function createLanguage({ description, exotic, name, script, typicalSpeakers }) {
  return await post('/languages', { description, exotic, name, script, typicalSpeakers })
}

export async function deleteLanguage(id) {
  return await _delete(`/languages/${id}`)
}

export async function getLanguage(id) {
  return await get(`/languages/${id}`)
}

export async function getLanguages({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/languages?${query}`)
}

export async function updateLanguage(id, { description, exotic, name, script, typicalSpeakers }) {
  return await put(`/languages/${id}`, { description, exotic, name, script, typicalSpeakers })
}
