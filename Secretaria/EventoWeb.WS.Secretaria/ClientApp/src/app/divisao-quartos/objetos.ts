import { DTOBasicoInscricaoResp } from '../inscricao/objetos';
import { EnumSexoQuarto } from '../quartos/objetos';

export class DTODivisaoQuarto {
  Id: number;
  Nome: string
  EhFamilia: boolean;
  Sexo: EnumSexoQuarto;
  Capacidade: number;
  Coordenadores: DTOBasicoInscricaoResp[];
  Participantes: DTOBasicoInscricaoResp[]
}
