import { _delete, get, post, put } from '.'

export async function createRace({
  attributes,
  attributesText,
  description,
  extraAttributes,
  extraLanguages,
  languageIds,
  languagesText,
  name,
  speedText,
  speeds
}) {
  return await post('/races', { attributes, attributesText, description, extraAttributes, extraLanguages, languageIds, languagesText, name, speedText, speeds })
}

export async function deleteRace(id) {
  return await _delete(`/races/${id}`)
}

export async function getRace(id) {
  return await get(`/races/${id}`)
}

export async function getRaces({ deleted, search, sort, desc, index, count }) {
  const query = Object.entries({ deleted, search, sort, desc, index, count })
    .filter(([, value]) => typeof value !== 'undefined' && value !== null)
    .map(pair => pair.join('='))
    .join('&')
  return await get(`/races?${query}`)
}

export async function updateRace(
  id,
  { attributes, attributesText, description, extraAttributes, extraLanguages, languageIds, languagesText, name, speedText, speeds }
) {
  return await put(`/races/${id}`, {
    attributes,
    attributesText,
    description,
    extraAttributes,
    extraLanguages,
    languageIds,
    languagesText,
    name,
    speedText,
    speeds
  })
}
